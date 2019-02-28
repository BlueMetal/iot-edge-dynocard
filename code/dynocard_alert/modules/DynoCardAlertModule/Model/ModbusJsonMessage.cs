using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class ModbusJsonMessage
    {
        public List<ModbusJsonContent> Content { get; set; }
        public DateTime PublishTimestamp { get; set; }
    }

    public class ModbusJsonContent
    {
        [JsonProperty("HwId")]
        public string HardwareId { get; set; }

        public List<ModbusJsonData> Data { get; set; }
    }

    public class ModbusJsonData
    {
        public string CorrelationId { get; set; }
        public DateTime SourceTimestamp { get; set; }
        public List<ModbusRegisterValue> Values { get; set; }
    }
}