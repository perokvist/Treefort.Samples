using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPS.Game.Domain;
using Treefort.Events;
using Treefort.Read;

namespace RPS.Game.ReadModel
{
    public class AvailableGames : IgnoreNonApplicableEventsAsync, IProjection
    {
        private readonly ConcurrentDictionary<Guid, ReadModel.Game> _games;

        public AvailableGames()
        {
            _games = new ConcurrentDictionary<Guid, Game>();
        }

        public IEnumerable<ReadModel.Game> GetGames()
        {
            return _games.Select(x => x.Value).ToList();
        }

        public Task WhenAsync(IEvent @event)
        {
            return HandleAsync((dynamic)@event);
        }

        public Task HandleAsync(GameCreatedEvent @event)
        {
            _games.GetOrAdd(@event.GameId, new ReadModel.Game {GameId = @event.GameId, Name = @event.GameName});
            return Task.FromResult(new object());
        }

        public Task HandleAsync(GameEndedEvent @event)
        {
            Game game;
            if (_games.ContainsKey(@event.GameId))
                _games.TryRemove(@event.GameId, out game);
            return Task.FromResult(new object());
        }
    }
}