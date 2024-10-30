using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpecFlow.RandomApi.Test.Helpers
{
    public static class JsonHelper
    {
        public static string JsonPrettify(this string json)
        {
            using var jDoc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
