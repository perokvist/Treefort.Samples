using System.Collections.Generic;
using RPS.Game.Domain.Public;

namespace RPS.Game.ReadModel
{
    public class ReadService : IReadService
    {
        private readonly AwailableGames _awailableGames;
        private readonly EndendGames _endendGames;

        public ReadService(AwailableGames awailableGames, EndendGames endendGames)
        {
            _awailableGames = awailableGames;
            _endendGames = endendGames;
        }

        public IEnumerable<Game> AwailableGames { get { return _awailableGames.GetGames(); }
        }
        public IEnumerable<EndedGame> EndedGames { get { return _endendGames.GetGames(); }
        }
    }
}