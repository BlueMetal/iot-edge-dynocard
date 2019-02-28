using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DynoCardAlertModule.Model
{
    public class OpcNodeReading
    {
        [JsonProperty("NodeId")]
        public string NodeId { get; set; }

        [JsonProperty("ApplicationUri")]
        public string ApplicationUri { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("Value")]
        public OpcNodeReadingValue Value { get; set; }

        // [JsonProperty("Value")]
        // public string Value { get; set; }

        // [JsonProperty("SourceTimestamp")]
        // public string SourceTimestamp { get; set; }
    }

    public class OpcNodeReadingValue
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("SourceTimestamp")]
        public string SourceTimestamp { get; set; }
    }
}