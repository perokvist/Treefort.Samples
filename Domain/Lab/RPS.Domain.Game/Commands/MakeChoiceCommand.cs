using System;
using RPS.Domain.Game.Domain;

namespace RPS.Domain.Game.Commands
{
    public class MakeChoiceCommand : IGameCommand
    {
        public MakeChoiceCommand(Guid gameId, String playerId, Choice choice)
        {
            GameId = gameId;
            PlayerId = playerId;
            Choice = choice;
            AggregateId = GameId;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; private set; }
        public Choice Choice { get; private set; }

        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; private set; }
    }
}