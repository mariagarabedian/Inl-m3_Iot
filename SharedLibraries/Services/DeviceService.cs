using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MAD = Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using SharedLibraries.Models;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace SharedLibraries.Services
{
    public static class DeviceService
    {
        //private static readonly Random rnd = new Random();
        private static HttpClient _client;

        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
            

            while (true)
            {
                /*
                var data = new TemperatureModel
                {
                    Temperature = rnd.Next(20,30),
                    Humidity = rnd.Next(40,50),
                   
                };*/

               
                _client = new HttpClient(); 
               
                var response = await _client.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Kumla,se&APPID=340a1c7e1eb2c2fac4b365398b20c7e8");
                var data = JsonConvert.DeserializeObject<TemperatureModel>(await response.Content.ReadAsStringAsync());
                var weather = new WeatherModel
                {
                    Temperature = data.main.temp,
                    Humidity = data.main.humidity
                };

                var json = JsonConvert.SerializeObject(weather);

                var payload = new Message(Encoding.UTF8.GetBytes(json));
                await deviceClient.SendEventAsync(payload);

                Console.WriteLine($"Message sent: {json}");

                await Task.Delay(60 * 1000);
            }


        }


        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {
            while(true)
            {
                var payload = await deviceClient.ReceiveAsync();

                if (payload == null)
                    continue;

                Console.WriteLine($"Message Received: {Encoding.UTF8.GetString(payload.GetBytes())}");

                await deviceClient.CompleteAsync(payload);
            }


        }
       
        public static async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
        {
            var payload = new MAD.Message(Encoding.UTF8.GetBytes(message));
            await serviceClient.SendAsync(targetDeviceId, payload);
        }

        
        


    }
}
