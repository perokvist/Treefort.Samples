using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPS.Game.Domain;
using Treefort.Events;
using Treefort.Read;

namespace RPS.Api
{
    public class AllGames : IgnoreNonApplicableEventsAsync, IProjection
    {
        public AllGames()
        {
            Games = new Dictionary<Guid, string>();
        }

        public Dictionary<Guid, string> Games { get; set; }

        public Task WhenAsync(IEvent @event)
        {
            return HandleAsync((dynamic)@event);
        }

        public Task HandleAsync(GameCreatedEvent @event)
        {
            Games.Add(@event.GameId, @event.GameName);
            return Task.FromResult(new object());
        }

    }
}