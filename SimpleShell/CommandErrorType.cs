namespace SimpleShell
{
    public enum CommandErrorType
    {
        /// <summary>
        /// No errors encountered while running the command.
        /// </summary>
        None,

        /// <summary>
        /// The process running the command could not be started.
        /// </summary>
        StartError,

        /// <summary>
        /// The command encountered an error while running.
        /// </summary>
        ExecutionError,

        /// <summary>
        /// The timeout limit was reached before the command was completed.
        /// </summary>
        TimeOutError
    }
}
