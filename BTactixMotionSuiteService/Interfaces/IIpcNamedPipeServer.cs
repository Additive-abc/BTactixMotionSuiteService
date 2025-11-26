using System.IO.Pipes;

namespace BTactixMotionSuiteService.Interfaces
{
    public interface IIpcNamedPipeServer
    {
        event Action<string>? OnMessageReceived;

        Task StartServerAsync();

        Task SendMessageAsync(NamedPipeServerStream pipe, string message);

        void StopServer();

        Task RunAsync(CancellationToken ct);
    }
}
