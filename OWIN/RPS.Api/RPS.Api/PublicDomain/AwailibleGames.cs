using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPS.Game.Domain;
using Treefort.Events;
using Treefort.Read;

namespace RPS.Api.PublicDomain
{
    public class AwailableGames : IgnoreNonApplicableEventsAsync, IProjection
    {
        private readonly Dictionary<Guid, Game> _games;

        public AwailableGames()
        {
            _games = new Dictionary<Guid, Game>();
        }

        public IEnumerable<Game> GetGames()
        {
            return _games.Select(x => x.Value).ToList();
        }

        public Task WhenAsync(IEvent @event)
        {
            return HandleAsync((dynamic)@event);
        }

        public Task HandleAsync(GameCreatedEvent @event)
        {
            _games.Add(@event.GameId, new Game {GameId = @event.GameId, Name = @event.GameName});
            return Task.FromResult(new object());
        }

        public Task HandleAsync(GameEndedEvent @event)
        {
            if (_games.ContainsKey(@event.GameId))
                _games.Remove(@event.GameId);
            return Task.FromResult(new object());
        }
    }
}