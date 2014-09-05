using System;
using Treefort.Events;

namespace RPS.Domain.Game.Events
{
    public class RoundWonEvent : IEvent
    {
        public RoundWonEvent(Guid gameId, string playerId, int round)
        {
            GameId = gameId;
            PlayerId = playerId;
            Round = round;
            SourceId = GameId;
        }

        public Guid GameId { get; private set; }
        public string PlayerId { get; private set; }
        public int Round { get; private set; }
        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}