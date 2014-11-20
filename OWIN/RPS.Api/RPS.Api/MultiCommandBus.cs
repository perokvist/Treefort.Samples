using System;
using System.Linq;
using System.Threading.Tasks;
using Treefort.Commanding;
using Treefort.Common;

namespace RPS.Api
{
    public class MultiCommandBus : ICommandBus
    {
        private readonly ICommandBus[] _buses;

        public MultiCommandBus(params ICommandBus[] buses)
        {
            _buses = buses;
        }

        public void Send(System.Collections.Generic.IEnumerable<Treefort.Messaging.Envelope<ICommand>> commands)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(Treefort.Messaging.Envelope<ICommand> command)
        {
            return Task.WhenAll(_buses.Select(b => b.SendAsync(command)));
        }
    }
}