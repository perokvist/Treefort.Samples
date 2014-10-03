using System;
using Treefort.Commanding;

namespace RPS.Game.Domain
{

    public interface IGameCommand : ICommand
    {}

    public class CreateGameCommand : IGameCommand
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

    public class MakeMoveCommand : IGameCommand
    {
        
        public MakeMoveCommand(Guid gameId, Move move, string playerName)
        {
            GameId = gameId;
            Move = move;
            PlayerName = playerName;
            AggregateId = GameId;
            CorrelationId = Guid.NewGuid();
        }

        public Guid GameId { get; private set; }
        public Move Move { get; private set; }
        public string PlayerName { get; private set; }
        
        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; private set; }
    }

}