using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SimpleShell
{
    public static class Command
    {
        /// <summary>
        /// Starts a process that runs a shell command and returns when the process has completed.
        /// </summary>
        /// <param name="command">The shell command to run.</param>
        public static async Task<ICommandResult> Run(string command)
        {
            return await Run(command, int.MaxValue);
        }

        /// <summary>
        /// Starts a process that runs a shell command and returns when the process has completed or timed out.
        /// </summary>
        /// <param name="command">The shell command to run.</param>
        /// <param name="timeout">The maximum allowed time in milliseconds for the command to run before terminating the process.</param>
        /// <returns></returns>
        public static async Task<ICommandResult> Run(string command, int timeout)
        {
            ProcessStartInfo startInfo = GetStartInfo(command);
            return await Task.Run(() => RunProcess(startInfo, timeout)).ConfigureAwait(false);
        }
        

        private static ProcessStartInfo GetStartInfo(string command)
        {
            string shell;
            string terminator;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                shell = "cmd";
                terminator = "/C";
            }
            else
            {
                shell = "/bin/bash";
                terminator = "-c";
            }

            return new ProcessStartInfo()
            {
                FileName = shell,
                Arguments = $"{terminator} {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }

        private static Task<ICommandResult> RunProcess(ProcessStartInfo startInfo, int timeout)
        {
            var tcs = new TaskCompletionSource<ICommandResult>();

            void ReturnResult(ICommandResult result)
            {
                tcs.SetResult(result);
            }

            using (Process process = new Process())
            {
                process.Exited += (sender, eventArgs) => 
                {
                    int exitCode = process.ExitCode;
                    string stdOut = process.StandardOutput.ReadToEnd();
                    string stdErr = process.StandardError.ReadToEnd();
                    CommandErrorType errorType = exitCode == 0 ? CommandErrorType.None : CommandErrorType.ExecutionError;
                    ReturnResult(new CommandResult
                    {
                        ErrorType = errorType,
                        ExitCode = exitCode,
                        StandardError = stdErr,
                        StandardOutput = stdOut
                    });
                };

                process.EnableRaisingEvents = true;
                process.StartInfo = startInfo;

                if (!process.Start())
                {
                    ReturnResult(new CommandResult() { ErrorType = CommandErrorType.StartError });
                }

                if (!process.WaitForExit(timeout))
                {
                    ReturnResult(new CommandResult() { ErrorType = CommandErrorType.TimeOutError });
                    process.EnableRaisingEvents = false;
                    process.StandardOutput.Dispose();
                    process.StandardError.Dispose();
                    process.Kill();
                }
            }

            return tcs.Task;
        }
    }
}
