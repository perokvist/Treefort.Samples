using System;
using Treefort.Events;

namespace RPS.Domain.Game.Events
{
    public class GameStartedEvent : IEvent
    {
        public GameStartedEvent(Guid gameId, string playerOneId, string playerTwoId)
        {
            GameId = gameId;
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
            SourceId = GameId;
        }

        public Guid GameId { get; private set; }
        public string PlayerOneId { get; private set; }
        public string PlayerTwoId { get; private set; }

        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}