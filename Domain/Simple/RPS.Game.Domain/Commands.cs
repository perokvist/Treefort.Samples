using System;
using System.Configuration;
using Treefort.Commanding;

namespace RPS.Game.Domain
{
    public class CreateGameCommand : ICommand
    {
        
        public CreateGameCommand(Guid gameId, string playerName, string gameName, Move firstMove)
        {
            GameId = gameId;
            PlayerName = playerName;
            GameName = gameName;
            FirstMove = firstMove;
            AggregateId = GameId;
            CorrelationId = Guid.NewGuid();
        }
       
        public Guid GameId { get; private set; }
        public string PlayerName { get; private set; }
        public string GameName { get; private set; }
        public Move FirstMove { get; private set; }

        public Guid AggregateId { get; private set; }

        public Guid CorrelationId { get; private set; }
    }

    public class MakeMoveCommand : ICommand
    {
        
        public MakeMoveCommand(Guid gameId, Move move, string playerName)
        {
            GameId = gameId;
            Move = move;
            PlayerName = playerName;
            AggregateId = GameId;
            CorrelationId = Guid.NewGuid();
        }

        public Guid GameId { get; set; }
        public Move Move { get; set; }
        public string PlayerName { get; set; }

        public Guid AggregateId { get; private set; }

        public Guid CorrelationId { get; private set; }
    }

}