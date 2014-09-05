using System;
using Treefort.Events;

namespace RPS.Domain.Game.Events
{
    public class GameWonEvent : IEvent
    {

        public GameWonEvent(Guid gameId, string playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
            SourceId = GameId;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; private set; }
        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}