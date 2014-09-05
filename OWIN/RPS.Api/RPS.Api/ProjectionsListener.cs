using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treefort.Events;
using Treefort.Read;

namespace RPS.Api
{
    public class ProjectionsListener : IEventListener
    {
        private readonly IEnumerable<IProjection> _projections;

        public ProjectionsListener(params IProjection[] projections)
        {
            _projections = projections;
        }

        public Task ReceiveAsync(IEnumerable<IEvent> events)
        {
            return Task.WhenAll(events.SelectMany(e => _projections.Select(p => p.WhenAsync(e))));
        }
    }
}