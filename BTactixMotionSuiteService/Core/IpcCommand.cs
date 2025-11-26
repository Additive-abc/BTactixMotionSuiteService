using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class IpcCommand
    {
        public string Type { get; set; }
        public JsonElement Payload { get; set; }
    }
}
