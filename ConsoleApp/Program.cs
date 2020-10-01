using System;
using Microsoft.Azure.Devices.Client;
using SharedLibraries.Models;
using SharedLibraries.Services;

namespace ConsoleApp
{
    class Program
    {
        private static readonly string _conn = "HostName=ec-win20-min-iothub.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=FFKKKzILXO5ti7goQ1Jxzg1eaAuDpszzd/pO5hawu78=";
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);


        static void Main(string[] args)
        {
            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();

            DeviceService.ReceiveMessageAsync(deviceClient).GetAwaiter();

            Console.ReadKey();
       
        }
    }
}
