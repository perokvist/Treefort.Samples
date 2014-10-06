using System;

namespace RPS.Api.PublicDomain
{
    public class EndedGame
    {
        public string Name { get; set; }
        public Guid GameId { get; set; }
        public string Winner { get; set; }
    }
}