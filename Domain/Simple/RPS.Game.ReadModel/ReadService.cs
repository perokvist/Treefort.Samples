using System.Collections.Generic;

namespace RPS.Game.ReadModel
{
    public class ReadService : IReadService
    {
        private readonly AvailableGames _availableGames;
        private readonly EndendGames _endendGames;

        public ReadService(AvailableGames availableGames, EndendGames endendGames)
        {
            _availableGames = availableGames;
            _endendGames = endendGames;
        }

        public IEnumerable<Game> AvailableGames { get { return _availableGames.GetGames(); }
        }
        public IEnumerable<EndedGame> EndedGames { get { return _endendGames.GetGames(); }
        }
    }
}