namespace SimpleShell
{
    public interface ICommandResult
    {
        /// <summary>
        /// The type of error encountered when running a command.
        /// </summary>
        CommandErrorType ErrorType { get; }

        /// <summary>
        /// The exit code returned from the process running a command.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// The standard error produced when running a command.
        /// </summary>
        string StandardError { get; }

        /// <summary>
        /// The standard output produced when running a command.
        /// </summary>
        string StandardOutput { get; }
    }
}