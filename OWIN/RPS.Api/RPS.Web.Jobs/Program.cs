using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RPS.Game.Domain;
using Treefort.Application;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.Commanding;

namespace RPS.Web.Jobs
{
    public class Program
    {
        static void Main()
        {
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            //Dispatcher for Commands (does nothing for now)
            var commandDispatcher = new Dispatcher<ICommand, Task>();
            commandDispatcher.Register<IGameCommand>(command => Task.Run(() => Console.WriteLine("Domain handeled command")));

            //Host and hooks
            var host = new JobHost();
            Func<ICommand, ICommandDispatcher, Task> handler = (command, dispatcher) => host.CallAsync(typeof(Program).GetMethod("Handle"),
            new {command, dispatcher });

            //Command processor
            var sessionReciver = new SessionQueueReceiver(connectionString, "commands-queue", Console.WriteLine);
            var dispatcherWrapper = new CommandDispatcherAction(commandDispatcher.Dispatch);
            var processor = new CommandProcessor(sessionReciver, new CommandDispatcherAction(command => handler(command, dispatcherWrapper)), new JsonTextSerializer());

            processor.Start();

            //Web Jobs Run
            host.RunAndBlock();
        }

        [NoAutomaticTrigger]
        public static async void Handle(ICommand command, ICommandDispatcher dispatcher, TextWriter logger)
        {
            logger.WriteLine("Recieved {0}", command.AggregateId);
            await dispatcher.DispatchAsync(command);
        }
    }
}
