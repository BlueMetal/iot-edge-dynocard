using System;
using System.Collections.Generic;
using System.Text;

namespace data_api.Models
{
    public class AnomalyEvent
    {
        public string Pump_ID { get; set; }
        public string Event_ID { get; set; }
        public string CardHeader_ID { get; set; }
        public string Card_Type { get; set; }
        public string EPOC_DATE { get; set; }
        public string Card_ID { get; set; }
        public string Position { get; set; }
        public string Load { get; set; }
    }
}
