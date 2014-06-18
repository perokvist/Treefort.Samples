using Microsoft.Azure.Jobs;
using Microsoft.ServiceBus.Messaging;
using Treefort.Commanding;
using Treefort.Infrastructure;

namespace RPS.Processor.WebJobsHost
{
    class Program
    {
        private static ICommandDispatcher _dispatcher;
        private static ITextSerializer _serializer;

        private static void Main(string[] args)
        {
            _serializer = Bootstrapper.GetSerializer();
            _dispatcher = Bootstrapper.GetDispatcher();
            new JobHost().RunAndBlock();
        }

        public static void ProcessCommand([ServiceBus("commands")] BrokeredMessage message)
        {
            _dispatcher.DispatchAsync(message.GetCommand(_serializer));
        }
    }
}
