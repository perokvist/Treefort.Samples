using System;
using System.Configuration;
using System.Threading.Tasks;
using Treefort.Application;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.Commanding;
using Treefort.Infrastructure;

namespace RPS.Processor
{
    public class Bootstrapper
    {
        public static ITextSerializer GetSerializer()
        {
            return new JsonTextSerializer();
        }

        public static ICommandDispatcher GetDispatcher()
        {
            var commandDispatcher = new Dispatcher<object, Task>();
            commandDispatcher.Register<Game.CreateGameCommand>(command => Task.Run(() => GameHandler.handle(command)));
            return new CommandDispatcherAction(commandDispatcher.Dispatch);
        }

        public static void Start(string connectionString)
        {
            const string path = "commands";

            var processor = new CommandProcessor(new QueueReceiver(connectionString, path),
                GetDispatcher(), GetSerializer());
            processor.Start();
        }

        public static void Start(Func<ICommandDispatcher> dispatcher, Func<ITextSerializer> serializer)
        {
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
           
            const string path = "commands";

            var processor = new CommandProcessor(new QueueReceiver(connectionString, path),
                dispatcher(), serializer());
            processor.Start();
        }
    }
}
