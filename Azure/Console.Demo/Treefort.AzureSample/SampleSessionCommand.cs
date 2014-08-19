using System;
using Treefort.Commanding;
using Treefort.Messaging;

namespace Treefort.AzureSample
{
    public class SampleSessionCommand : ICommand, ISessionMessage
    {



        public SampleSessionCommand() : this(Guid.NewGuid())
        {
        }

        public SampleSessionCommand(Guid id)
        {
            AggregateId = id;
            CorrelationId = Guid.NewGuid();
        }

        public Guid AggregateId { get; set; }

        public Guid CorrelationId { get; set; }

        public string SessionId
        {
            get { return AggregateId.ToString(); }
        }
    }
}