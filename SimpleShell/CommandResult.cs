namespace SimpleShell
{
    internal class CommandResult : ICommandResult
    {
        public CommandErrorType ErrorType { get; internal set; } = CommandErrorType.None;
        public int ExitCode { get; internal set; } = -1;
        public string StandardError { get; internal set; } = string.Empty;
        public string StandardOutput { get; internal set; } = string.Empty;
    }
}
