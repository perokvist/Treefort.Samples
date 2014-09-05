using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RPS.Game.Domain;
using Treefort.Common;
using Treefort.Events;
using Treefort.Read;

namespace RPS.Api
{
    public class AwailibleGames : IgnoreNonApplicableEventsAsync, IProjection
    {

        public AwailibleGames()
        {
            Games = new Dictionary<Guid, string>();
        }

        public Dictionary<Guid, string> Games { get; set; }

        public Task WhenAsync(IEvent @event)
        {
            return Handle((dynamic)@event);
        }

        //TODO rename in baseclass to HandleAsync
        public Task Handle(GameCreatedEvent @event)
        {
            Games.Add(@event.GameId, @event.GameName);
            return Task.FromResult(new object());
        }

        public Task Handle(GameEndedEvent @event)
        {
            if (Games.ContainsKey(@event.GameId))
                Games.Remove(@event.GameId);
            return Task.FromResult(new object());
        }
    }

}