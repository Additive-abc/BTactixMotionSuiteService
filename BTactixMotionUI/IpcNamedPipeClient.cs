using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionUI
{
    public class IpcNamedPipeClient
    {
        private const string PipeName = "BTactixPipe";

        public async Task<bool> PingAsync()
        {
            return await AppServices.ErrorHandler.ExecuteAsync(async () =>
            {
                using var client = new NamedPipeClientStream(
                    serverName: ".",
                    pipeName: PipeName,
                    direction: PipeDirection.InOut,
                    options: PipeOptions.Asynchronous
                );

                // 1) Try connecting (2-second timeout)
                await client.ConnectAsync(2000);

                // 2) Send PING request
                var pingBytes = Encoding.UTF8.GetBytes("PING");
                await client.WriteAsync(pingBytes, 0, pingBytes.Length);
                await client.FlushAsync();

                // 3) Wait for server response (optional)
                var buffer = new byte[256];
                var bytesRead = await client.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    var reply = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AppServices.AppLogger.Info($"Pipe responded: {reply}");

                    return reply == "PONG";
                }

                return false;

            }, AppServices.AppLogger, "PingAsync");
        }


        public async Task SendMessageAsync(string msg)
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
            await client.ConnectAsync(2000);

            var buffer = Encoding.UTF8.GetBytes(msg);
            await client.WriteAsync(buffer, 0, buffer.Length);
            await client.FlushAsync();
        }
    }
}
