using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Core;
using BTactixMotionSuiteService.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Services
{
    public class DeviceManagementService :BaseService, IDeviceManagementService
    {
        private readonly ConcurrentDictionary<string, DeviceInfo> _devices = new();

        public DeviceManagementService(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler)
         : base(appLoggerFactory, errorHandler)
        {
            // seed some fake devices for demo - replace with BLE/USB scanning
            for (int i = 1; i <= 2; i++)
            {
                var d = new DeviceInfo { Id = $"bg-{i}", Name = $"BGlove-{i}", BatteryPercent = 90 - i * 5, Connected = false };
                _devices[d.Id] = d;
            }
        }

        public IEnumerable<DeviceInfo> GetDevices() => _devices.Values.ToArray();

        public async Task<DeviceInfo?> ConnectAsync(string id, CancellationToken ct)
        {
            return await ErrorHandler.ExecuteAsync(async () =>
            {
                if (!_devices.TryGetValue(id, out var dev)) return null;
                // Simulate connecting and start per-device monitoring
                await Task.Delay(300, ct);
                dev.Connected = true;
                _devices[id] = dev;
                return dev;

            }, Logger, nameof(ConnectAsync));
        }

        public async Task DisconnectAsync(string id)
        {
            _ = ErrorHandler.ExecuteAsync(async () =>
            {
                if (_devices.TryGetValue(id, out var d))
                {
                    d.Connected = false;
                    _devices[id] = d;
                    await Task.CompletedTask;
                }
            }, Logger, nameof(DisconnectAsync));
        }
    }
}
