using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Core;
using BTactixMotionSuiteService.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Services
{
    public class DataExportService : BaseService, IDataExportService
    {
        private readonly IEventBus _bus;
        private UdpClient? _udp;
        private IPEndPoint? _udpEndpoint;

        public DataExportService(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler, IEventBus bus)
         : base(appLoggerFactory, errorHandler)
        {
            _bus = bus;
            // configure destination statically for now
            _udpEndpoint = new IPEndPoint(IPAddress.Loopback, 9000);
            _udp = new UdpClient();
            _bus.Subscribe<GloveFrame>(OnFrame);
        }

        public void Start()
        {
            // nothing to start for UDP
        }

        private void OnFrame(GloveFrame f)
        {
            ErrorHandler.Execute(() =>
            {
                if (_udp == null || _udpEndpoint == null) return;
                var s = $"TS:{f.TimestampMs},L0:{f.LeftFingerFlex[0]:F2},L1:{f.LeftFingerFlex[1]:F2}";
                var b = Encoding.UTF8.GetBytes(s);
                _udp.SendAsync(b, b.Length, _udpEndpoint);

            }, Logger, nameof(OnFrame));
        }

        public void Stop()
        {
            _udp?.Close();
            _udp = null;
        }
    }
}
