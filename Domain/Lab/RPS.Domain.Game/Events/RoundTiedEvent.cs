using System;
using Treefort.Events;

namespace RPS.Domain.Game.Events
{
    public class RoundTiedEvent : IEvent
    {
        public RoundTiedEvent(Guid gameId, int round)
        {
            GameId = gameId;
            Round = round;
            SourceId = GameId;
        }

        public Guid GameId { get; private set; }
        public int Round { get; private set; }
        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}