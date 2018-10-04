using DynoCardWebAPI.Models;
using System;
using System.Threading.Tasks;
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
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(settings.ConnectionStrings.DeviceConnectionString);

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
