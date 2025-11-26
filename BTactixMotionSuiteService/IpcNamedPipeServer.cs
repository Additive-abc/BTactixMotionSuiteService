using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Core;
using BTactixMotionSuiteService.Interfaces;
using log4net.Core;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace BTactixMotionSuiteService
{
    public class IpcNamedPipeServer : BaseService, IIpcNamedPipeServer
    {
        private const string PipeName = "BTactixPipe";
        private readonly CancellationTokenSource _cts = new();
        public event Action<string>? OnMessageReceived;
        public event Action<GloveFrame>? OnFrameReceived;
        public event Action<string>? OnCommandReceived;
        private readonly IEventBus _bus;


        public IpcNamedPipeServer(IAppLoggerFactory loggerFactory, BTactix.Common.Interfaces.IErrorHandler errorHandler, IEventBus bus) : base(loggerFactory, errorHandler)
        {
            _bus = bus;
        }

        public async Task StartServerAsync()
        {
            _ = ErrorHandler.ExecuteAsync(async () =>
            {
                Logger.Info("Named Pipe Server starting...");

                while (!_cts.Token.IsCancellationRequested)
                {
                    var server = new NamedPipeServerStream(
                        PipeName,
                        PipeDirection.InOut,
                        maxNumberOfServerInstances: 5,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous);

                    await server.WaitForConnectionAsync(_cts.Token);

                    Logger.Info("Client connected.");
                    // Handle the client without blocking the accept loop
                    _ = HandleClientSafeAsync(server);
                }

            }, Logger, "Named Pipe StartServer");
        }

        public void StopServer() => _cts.Cancel();

        private async Task HandleClientSafeAsync(NamedPipeServerStream pipe)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                using (pipe)
                {
                    await HandleClientAsync(pipe);
                }
            }, Logger, "HandleClient");
        }

        private async Task HandleClientAsync(NamedPipeServerStream pipe)
        {
            var buffer = new byte[8192];

            while (pipe.IsConnected)
            {
                var bytesRead = await pipe.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead <= 0) break;

                string raw = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // A single read may contain multiple messages → split by newline.
                var messages = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (string msg in messages)
                {
                    ProcessIncoming(pipe, msg.Trim());
                }
            }
        }

        private void ProcessIncoming(NamedPipeServerStream pipe, string msg)
        {
            _ = ErrorHandler.ExecuteAsync(async () =>
            {
                OnMessageReceived?.Invoke(msg);

                if (msg == "PING")
                {
                    _ = SendMessageAsync(pipe, "PONG\n");
                    return;
                }

                if (msg.StartsWith("{"))
                {
                    var frame = JsonSerializer.Deserialize<GloveFrame>(msg);
                    if (frame != null)
                    {
                        OnFrameReceived?.Invoke(frame);
                        return;
                    }
                }

                OnCommandReceived?.Invoke(msg);
            }, Logger, "ProcessIncoming");
        }

        public Task SendMessageSafeAsync(NamedPipeServerStream pipe, string message)
        {
            return ErrorHandler.ExecuteAsync(async () =>
            {
                await SendMessageAsync(pipe, message);
            }, Logger, "SendMessage");
        }

        public async Task SendMessageAsync(NamedPipeServerStream pipe, string message)
        {
            if (pipe?.IsConnected == true)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await pipe.WriteAsync(buffer, 0, buffer.Length);
                await pipe.FlushAsync();
            }
        }

        public async Task RunAsync(CancellationToken ct)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                Logger.Info("IpcNamedPipeServer RunAsync started.");
                // Start your existing infinite-loop server
                _ = StartServerAsync();

                // Wait until cancellation
                await Task.Delay(Timeout.Infinite, ct);

                // Stop everything
                StopServer();
                Logger.Info("IpcNamedPipeServer stopped.");

            }, Logger, nameof(RunAsync));            
        }

    }
}