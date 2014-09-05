using System;
using Treefort.Events;
namespace RPS.Game.Domain
{
   public class MadeMoveEvent : IEvent
   {
       public MadeMoveEvent(Guid gameId, Move move, string playerName)
       {
           GameId = gameId;
           Move = move;
           PlayerName = playerName;
           SourceId = GameId;
       }

       public string PlayerName { get; private set; }
       public Move Move { get; private set; }
       public Guid GameId { get; private set; }

       public Guid CorrelationId { get; set; }
       public Guid SourceId { get; private set; }
   } 

    public class GameCreatedEvent : IEvent
    {
        
        public GameCreatedEvent(Guid gameId, string gameName, string playerName)
        {
            GameId = gameId;
            GameName = gameName;
            PlayerName = playerName;
            SourceId = GameId;
        }

        public Guid GameId { get; private set; }
        public string GameName { get; private set; }
        public string PlayerName { get; private set; }

        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }

        public string Move { get; set; }
    }
    public class GameEndedEvent : IEvent
    {
        public GameEndedEvent(Guid gameId, GameResult result, Tuple<string,string> players)
        {
            GameId = gameId;
            Result = result;
            Players = players;
            SourceId = GameId;
        }
       
        public Guid GameId { get; set; }
        public GameResult Result { get; set; }
        public Tuple<string, string> Players { get; set; }

        public Guid CorrelationId { get; set; }
        public Guid SourceId { get; private set; }
    }
}