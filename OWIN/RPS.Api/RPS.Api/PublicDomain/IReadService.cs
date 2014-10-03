using System.Collections.Generic;

namespace RPS.Api.PublicDomain
{
    public interface IReadService
    {
        IEnumerable<Game> AwailableGames { get; }
        IEnumerable<EndedGame> EndedGames { get; }  
    }
}