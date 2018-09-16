using System.Threading.Tasks;
using Xunit;

namespace SimpleShell.Tests
{
    public class Tests
    {
        [Fact]
        public async void TestEcho()
        {
            ICommandResult result = await Command.Run("echo test");
            Assert.True(result.ErrorType == CommandErrorType.None);
            Assert.True(result.ExitCode == 0);
            Assert.Empty(result.StandardError);
            Assert.Contains("test", result.StandardOutput);
        }

        [Fact]
        public async void TestInvalidCommand()
        {
            ICommandResult result = await Command.Run("invalid command");
            Assert.True(result.ErrorType == CommandErrorType.ExecutionError);
            Assert.True(result.ExitCode != 0);
            Assert.Empty(result.StandardOutput);
            Assert.NotEmpty(result.StandardError);
        }

        [Fact]
        public async void TestTimeout()
        {
            ICommandResult result = await Command.Run("ping 127.0.0.1", 100);
            Assert.True(result.ErrorType == CommandErrorType.TimeOutError);
            Assert.True(result.ExitCode != 0);
            Assert.Empty(result.StandardError);
            Assert.Empty(result.StandardOutput);
        }

        [Fact]
        public void TestNonBlocking()
        {
            Task<ICommandResult> task = Command.Run("ping 127.0.0.1", 100);
            Assert.True(task.Status != TaskStatus.RanToCompletion);
            task.Wait();
            Assert.True(task.Status == TaskStatus.RanToCompletion);
        }
    }
}
