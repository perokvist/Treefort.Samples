using System.IO;
using Microsoft.ServiceBus.Messaging;
using Treefort.Infrastructure;
using Treefort.Commanding;

namespace RPS.Processor
{
    public static class MessageExtensions
    {
        //TODO add to Treefort
        public static ICommand GetCommand(this BrokeredMessage message, ITextSerializer serializer)
        {
            object payload;

            using (var stream = message.GetBody<Stream>())
            {
                using (var reader = new StreamReader(stream))
                {
                    payload = serializer.Deserialize(reader);
                }
            }

            return payload as ICommand;
        } 
    }
}