using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core.Gesture
{
    public static class GestureProfileLoader
    {
        public static GestureProfile Load(string path)
        {
            var s = File.ReadAllText(path);
            return JsonSerializer.Deserialize<GestureProfile>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }
    }
}
