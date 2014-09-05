using System;

namespace RPS.Domain.Game.Commands
{
    public class CreateGameCommand : IGameCommand
    {

        public CreateGameCommand(Guid gameId, string playerId, string title, int firstTo)
        {
            GameId = gameId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
            AggregateId = GameId;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; private set; }
        public string Title { get; private set; }
        public int FirstTo { get; private set; }

        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; private set; }
    }
}
