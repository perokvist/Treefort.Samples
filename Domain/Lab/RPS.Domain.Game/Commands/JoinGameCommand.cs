using System;

namespace RPS.Domain.Game.Commands
{
    public class JoinGameCommand : IGameCommand
    {
        public JoinGameCommand(Guid gameId, string playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
            AggregateId = GameId;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; private set; }

        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; private set; }
    }
}