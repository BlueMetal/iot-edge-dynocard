using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class DynoCardAnomalyResult
    {
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Anomaly { get; set; }
    }
}