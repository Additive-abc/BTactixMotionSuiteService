using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionUI
{
    public class HttpServiceClient
    {
        private readonly HttpClient httpClient;

        public HttpServiceClient(string baseUrl)
        {
            httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

        }

        public async Task<bool> PingServiceAsync()
        {
            return await AppServices.ErrorHandler.ExecuteAsync(async () =>
            {
                var response = await httpClient.GetAsync("/");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                AppServices.AppLogger.Info($"Service responded: {content}");
                return true;

            }, AppServices.AppLogger, "PingServiceAsync");
        }
    }
}
