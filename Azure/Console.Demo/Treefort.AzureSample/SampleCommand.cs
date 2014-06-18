using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treefort.Application;
using Treefort.Commanding;
using Treefort.Infrastructure;

namespace Treefort.AzureSample
{
    public class SampleCommand : ICommand
    {
        public SampleCommand()
        {
            AggregateId = Guid.NewGuid();
            CorrelationId = Guid.NewGuid();
        }

        public Guid AggregateId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
