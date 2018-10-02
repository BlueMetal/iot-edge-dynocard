using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynoCardAlertModule.Config;
using DynoCardAlertModule.Data;
using System.Threading.Tasks;

namespace DynoCardAlertModule.Model
{
    public class StopModbusMessage
    {
        public string Status { get; set; }
    }
}