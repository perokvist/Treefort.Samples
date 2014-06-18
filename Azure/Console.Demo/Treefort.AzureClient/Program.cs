using System;
using System.Configuration;
using Microsoft.ServiceBus;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.AzureSample;
using Treefort.Commanding;

namespace Treefort.AzureClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var manager = NamespaceManager.CreateFromConnectionString(connectionString);
            const string path = "commands";
            if(!manager.QueueExists(path))
                manager.CreateQueue(path);
            var bus = new CommandBus(new QueueSender(connectionString, path), new JsonTextSerializer());
            for (var i = 0; i < 3; i++)
            {
                Console.WriteLine("Sending command {0}", i + 1);
                bus.SendAsync(new SampleCommand()).Wait();
            }
            Console.ReadLine();
        }
    }
}
