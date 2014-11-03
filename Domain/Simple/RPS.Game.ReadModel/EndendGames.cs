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
            var winner = string.Empty;
            if(@event.Result != GameResult.Tie)
                winner = @event.Result == GameResult.PlayerOneWin ? @event.Players.Item1 : @event.Players.Item2;

            _games.GetOrAdd(@event.GameId, new EndedGame {GameId = @event.GameId, Name = @event.GameName, Winner = winner, Result = @event.Result.ToString()});
            return Task.FromResult(0);
        }
    }

}