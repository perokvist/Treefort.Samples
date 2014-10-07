using System.Collections.Generic;

namespace RPS.Game.ReadModel
{
    public interface IReadService
    {
        IEnumerable<Game> AvailableGames { get; }
        IEnumerable<EndedGame> EndedGames { get; }  
    }
}