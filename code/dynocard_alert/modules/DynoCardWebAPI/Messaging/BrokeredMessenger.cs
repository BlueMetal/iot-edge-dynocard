using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using DynoCardWebAPI.Helpers;
using DynoCardWebAPI.Models;
using DynoCardWebAPI.Repos;
using Microsoft.Azure.Devices.Client;

namespace DynoCardWebAPI.Messaging
{
    public class BrokeredMessenger
    {
        private static ModuleClient _moduleClient;
        
        public static async Task InitAsync(string deviceConnectionString)
        {
            if (_moduleClient == null)
            {
                try
                {
                   
                    AmqpTransportSettings amqpTransportSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
                    ITransportSettings[] settings = { amqpTransportSetting };
                    // _moduleClient = ModuleClient.CreateFromConnectionString(deviceConnectionString, settings);
                    _moduleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);

                    await _moduleClient.OpenAsync();
                    Console.WriteLine("IoT Hub module client initialized.");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception occurred trying to create module client: {ex.Message}");
                }
            }
        }

        public static async Task Send(string message, string deviceConnectionString)
        {
            if (_moduleClient == null)
            {
                await InitAsync(deviceConnectionString);
            }

            var messageByteString = Encoding.UTF8.GetBytes(message);
            var deviceMessage = new Message(messageByteString);
            await _moduleClient.SendEventAsync("anomalyOutput", deviceMessage);
            System.Console.WriteLine($"Sent anomaly change event: {messageByteString}");
        }
    }
}