using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace TheGuarantorsChallenge.Helpers
{
    public static class StringContentHelper
    {
        public static StringContent GetJSONStringContent<T>(T content) => new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
    }
}
