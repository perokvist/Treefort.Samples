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
          
            //SendMessages(bus, 3);
            SendSessionMessages(bus, 3);

            Console.ReadLine();
        }

        private static void SendSessionMessages(CommandBus bus, int count)
        {
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine("Sending session command {0}", i + 1);
                bus.SendAsync(new SampleSessionCommand()).Wait();
            }
        }

        private static void SendMessages(CommandBus bus, int count)
        {
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine("Sending command {0}", i + 1);
                bus.SendAsync(new SampleCommand()).Wait();
            }
        }
    }
}
