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
    public class EndendGames : IgnoreNonApplicableEventsAsync, IProjection
    {
        private readonly ConcurrentDictionary<Guid, EndedGame> _games;

        public EndendGames()
        {
            _games = new ConcurrentDictionary<Guid, EndedGame>();
        }

        public IEnumerable<EndedGame> GetGames()
        {
            return _games.Select(x => x.Value).ToList();
        }

        public Task WhenAsync(IEvent @event)
        {
            return HandleAsync((dynamic)@event);
        }

        public Task HandleAsync(GameEndedEvent @event)
        {
            _games.GetOrAdd(@event.GameId, new EndedGame {GameId = @event.GameId, Name = @event.GameName , Winner = @event.Result.ToString()});
            return Task.FromResult(0);
        }
    }

}