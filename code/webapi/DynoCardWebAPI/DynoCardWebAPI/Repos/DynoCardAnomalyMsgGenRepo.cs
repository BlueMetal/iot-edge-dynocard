using DynoCardWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using DynoCardWebAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace DynoCardWebAPI.Repos
{
    public class DynoCardAnomalyMsgGenRepo : IDynoCardAnomalyMsgGenRepo
    {
        private Settings settings { get; set; }
        public DynoCardAnomalyMsgGenRepo(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }

        public void Send(DynoCardAnomalyEvent dcae)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(settings.DeviceConnectionString);

            if (deviceClient == null)
            {
                throw new Exception("Failed to create device client.  Please check the connection string and ensure that the device is registered.");
            }

            SendEvent(deviceClient, dcae).Wait();
        }

        private static async Task SendEvent(DeviceClient deviceClient, DynoCardAnomalyEvent dcae)
        {
            string msgBody = JsonConvert.SerializeObject(dcae);
            Message eventMessage = new Message(Encoding.UTF8.GetBytes(msgBody));

            await deviceClient.SendEventAsync(eventMessage).ConfigureAwait(false);
        }

    }
}
