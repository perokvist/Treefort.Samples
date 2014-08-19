using System;
using System.Configuration;
using System.Threading.Tasks;
using Treefort.Application;
using Treefort.Azure;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.AzureSample;
using Treefort.Commanding;
using Treefort.Infrastructure;

namespace Treefort.AzureProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var logger = new ConsoleLogger();
            var store = new InMemoryEventStore(() => new InMemoryEventStream());
            var dispatcher = new Dispatcher<ICommand, Task>();
            dispatcher.Register<SampleCommand>(command => Task.Run(() => Console.WriteLine("Received {0}", command.AggregateId)));
            dispatcher.Register<SampleSessionCommand>(command => Task.Run(() => Console.WriteLine("Received Session Command {0} Session: {1}", command.AggregateId, command.SessionId)));

            const string path = "commands";

            var processor = SessionCommandProcessor(connectionString, path, dispatcher); //CommandProcessor(connectionString, path, dispatcher);
            
            processor.Start();
            Console.ReadLine();
            processor.Stop();
        }

        private static IProcessor CommandProcessor(string connectionString, string path, Dispatcher<ICommand, Task> dispatcher)
        {
            var processor = new CommandProcessor(new QueueReceiver(connectionString, path),
                new CommandDispatcherAction(dispatcher.Dispatch), new JsonTextSerializer());
            return processor;
        }

        private static IProcessor SessionCommandProcessor(string connectionString, string path, Dispatcher<ICommand, Task> dispatcher)
        {
            var processor = new CommandProcessor(new SessionQueueReceiver(connectionString, path),
                new CommandDispatcherAction(dispatcher.Dispatch), new JsonTextSerializer());
            return processor;
        }

    }
}
