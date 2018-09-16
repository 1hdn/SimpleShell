# SimpleShell
SimpleShell provides a simplified interface for running shell commands using the async/await pattern. 



```c#

ICommandResult result = await Command.Run("echo hello");

CommandErrorType errorType = result.ErrorType;  // CommandErrorType.None
int exitCode = result.ExitCode;                 // 0
string error = result.StandardError;            // string.Empty
string output = result.StandardOutput;          // "hello"

```

The `Command.Run` method is best suited for simple shell commands like executing a script or starting a program. 
Each invocation of `Command.Run` starts a new process and returns when the process has exited. 

If you need to interact with a script or program (like sending input based on output) this is not the right tool for the job. 

SimpleShell is available as a [NuGet package](https://www.nuget.org/packages/SimpleShell/) for .NET Standard 2.0.
