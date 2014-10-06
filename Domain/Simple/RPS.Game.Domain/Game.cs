using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Treefort.Events;

namespace RPS.Game.Domain
{
    public class Game
    {
        private readonly GameState _state;

        public Game(GameState state)
        {
            _state = state;
        }

        private GameResult Wins(Move playerOneMove, Move playerTwoMove)
        {
            var dict = new Dictionary<Tuple<Move, Move>, GameResult>
            {
                {new Tuple<Move, Move>(Move.Paper, Move.Scissors), GameResult.PlayerTwoWin},
                {new Tuple<Move, Move>(Move.Rock, Move.Paper), GameResult.PlayerTwoWin},
                {new Tuple<Move, Move>(Move.Scissors, Move.Rock), GameResult.PlayerTwoWin}
            };

            if (playerOneMove == playerTwoMove)
                return GameResult.Tie;

            GameResult playerTwoWins;
            return dict.TryGetValue(new Tuple<Move, Move>(playerOneMove, playerTwoMove), out playerTwoWins) ? playerTwoWins : GameResult.PlayerOneWin;
        }

        private bool IsValidPlayer(string playerName)
        {
            return _state.CreatorName != playerName;
        }
        
        public IEnumerable<IEvent> Handle(IGameCommand command)
        {
            return this.Handle((dynamic) command);
        } 

        public IEnumerable<IEvent> Handle(CreateGameCommand command)
        {
            if (_state.Status != GameStatus.NotStarted)
                return Enumerable.Empty<IEvent>();

            return new List<IEvent>
            {
                new GameCreatedEvent(command.GameId, command.GameName, command.PlayerName),
                new MadeMoveEvent(command.GameId, command.FirstMove, command.PlayerName)
            };
        }

        public IEnumerable<IEvent> Handle(MakeMoveCommand command)
        {
            if (_state.Status != GameStatus.Started)
                return Enumerable.Empty<IEvent>();

            if(!IsValidPlayer(command.PlayerName))
                return Enumerable.Empty<IEvent>();
            
            var result = Wins(_state.CreatorMove, command.Move);

            return new List<IEvent>
                {
                    new MadeMoveEvent(command.GameId, command.Move, command.PlayerName),
                    new GameEndedEvent(command.GameId, result, new Tuple<string, string>(_state.CreatorName, command.PlayerName), _state.GameName)
                };

        }
    }
}