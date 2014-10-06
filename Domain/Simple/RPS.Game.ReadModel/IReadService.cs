using System.Collections.Generic;

namespace RPS.Game.ReadModel
{
    public interface IReadService
    {
        IEnumerable<Game> AwailableGames { get; }
        IEnumerable<EndedGame> EndedGames { get; }  
    }
}