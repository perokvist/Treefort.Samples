using System;
using System.Net.Configuration;
using Treefort.Domain;
using Treefort.Events;

namespace RPS.Game.Domain
{
    public class GameState : IState
    {
        public void When(GameCreatedEvent @event)
        {
            Status = GameStatus.Started;
            CreatorName = @event.PlayerName;
            Move creatorMove;
            Enum.TryParse(@event.Move, out creatorMove);
            CreatorMove = creatorMove;
            GameName = @event.GameName;
        }

        public void When(MadeMoveEvent @event)
        {
            if (@event.PlayerName != CreatorName) return;
            Status = GameStatus.Started;
            CreatorName = @event.PlayerName;
            CreatorMove = @event.Move;
        }

        public void When(GameEndedEvent @event)
        {
            Status = GameStatus.Ended;
        }

        public GameStatus Status { get; private set; }

        public Move CreatorMove { get; private set; }

        public string GameName { get; private set; }

        public string CreatorName { get; private set; }
        
        public void When(IEvent @event)
        {

        }

        public Guid AggregateId { get; set; }
        public long Version { get; set; }
    }
}