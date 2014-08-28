using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.ServiceBus;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.AzureSample;
using Treefort.Commanding;
using Treefort.Common;
using Treefort.Messaging;

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
            //if(!manager.QueueExists(path))
            //    manager.CreateQueue(path);
            var bus = new CommandBus(new QueueSender(connectionString, path), new JsonTextSerializer());
          
            //SendMessages(bus, 3);
            var sessions = new List<Guid>() {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};

            sessions.ForEach(guid => SendSessionMessages(bus, 3, guid));
            Console.WriteLine("Sent 3x3 press any key to send more...");
            Console.ReadLine();
            sessions.ForEach(guid => SendSessionMessages(bus, 3, guid));
            Console.WriteLine("Sent 3x3 more... press any key to quit");
            Console.ReadLine();
        }

        private static void SendSessionMessages(CommandBus bus, int count, Guid session)
        {
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine("Sending session command {0}", i + 1);
                var cmd = new SampleSessionCommand(session);
                cmd.CastAction<ISessionMessage>(x => Console.WriteLine("Session cmd {0}", x.SessionId));
                bus.SendAsync(cmd).Wait();
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
