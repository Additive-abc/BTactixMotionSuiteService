using BTactixMotionSuiteService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Interfaces
{
    public interface IDeviceManagementService
    {
        IEnumerable<DeviceInfo> GetDevices();
        Task<DeviceInfo?> ConnectAsync(string id, CancellationToken ct);
        Task DisconnectAsync(string id);
    }
}
