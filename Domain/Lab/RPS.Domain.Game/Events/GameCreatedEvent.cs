using System;
using Treefort.Events;

namespace RPS.Domain.Game.Events
{
    public class GameCreatedEvent : IEvent
    {
        public GameCreatedEvent(Guid entityId, string playerId, string title, int firstTo, DateTime utcNow)
        {
            GameId = entityId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
            Created = utcNow;
            SourceId = GameId;
        }

        public Guid GameId { get; private set; }
        public string PlayerId { get; private set; }
        public string Title { get; private set; }
        public int FirstTo { get; private set; }
        public DateTime Created { get; private set; }

        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}
