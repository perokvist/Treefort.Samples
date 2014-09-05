using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RPS.Domain.Game.Commands;
using RPS.Domain.Game.Events;
using Treefort.Events;

namespace RPS.Domain.Game.Domain
{
    public class Game
    {
        private readonly GameState state;
        private readonly IDictionary<Choice, Choice> winnersAgainst;

        public Game(GameState state)
        {
            this.state = state;
            winnersAgainst = new ConcurrentDictionary<Choice, Choice>();
            winnersAgainst.Add(Choice.Rock, Choice.Paper);
            winnersAgainst.Add(Choice.Scissors, Choice.Rock);
            winnersAgainst.Add(Choice.Paper, Choice.Scissors);
        }

        public IEnumerable<IEvent> Handle(CreateGameCommand command)
        {
            return new List<IEvent>
                       {
                           new GameCreatedEvent(
                               command.GameId,
                               command.PlayerId,
                               command.Title,
                               command.FirstTo,
                               DateTime.UtcNow)
                       };
        }
        
        public IEnumerable<IEvent> Handle(JoinGameCommand command)
        {
            var events = new List<IEvent>();

            if (state.PlayerTwo == null)
            {
                events.Add(new GameStartedEvent(state.Id, state.PlayerOne.Email, command.PlayerId));
                events.Add(new RoundStartedEvent(state.Id, 1));
            }

            return events;
        }

        public IEnumerable<IEvent> Handle(MakeChoiceCommand command)
        {
            var events = new List<IEvent>();
            var playerOneChoice = state.PlayerOne.CurrentChoice;
            var playerTwoChoice = state.PlayerTwo.CurrentChoice;

            if (state.State != GameStatus.Started)
                return events;

            if (IsPlayerOne(command.PlayerId) && playerOneChoice == Choice.None)
                playerOneChoice = PlayerChoice(command, events);
            else if (IsPlayerTwo(command.PlayerId) && playerTwoChoice == Choice.None)
                playerTwoChoice = PlayerChoice(command, events);

            // Decide winner if both has chosen
            if (playerOneChoice != Choice.None && playerTwoChoice != Choice.None)
            {
                // decide round
                var newRound = true;
                Choice winsAgainstOne = winnersAgainst[state.PlayerOne.CurrentChoice];

                if (playerTwoChoice == winsAgainstOne)
                    newRound = RoundWonBy(state.PlayerTwo, command, events);
                else if (playerOneChoice == state.PlayerTwo.CurrentChoice)
                    events.Add(new RoundTiedEvent(command.GameId, state.Round));
                else
                    newRound = RoundWonBy(state.PlayerOne, command, events);

                if (newRound)
                    events.Add(new RoundStartedEvent(command.GameId, state.Round + 1));
            }
            return events;
        }


        private bool IsWinner(GamePlayer player)
        {
            return (player.Score + 1) >= state.FirstTo;
        }

        private void GameWonBy(GamePlayer player, MakeChoiceCommand command, ICollection<IEvent> events)
        {
            events.Add(new GameWonEvent(command.GameId, player.Email));
        }

        private bool RoundWonBy(GamePlayer player, MakeChoiceCommand command, ICollection<IEvent> events)
        {
            events.Add(new RoundWonEvent(command.GameId, player.Email, state.Round));
            if (IsWinner(state.PlayerTwo))
            {
                GameWonBy(state.PlayerTwo, command, events);
                return false;
            }
            return true;
        }

        private Choice PlayerChoice(MakeChoiceCommand command, ICollection<IEvent> events)
        {
            events.Add(new ChoiceMadeEvent(
                         command.GameId,
                         state.Round,
                         command.PlayerId,
                         command.Choice));
            var playerChoice = command.Choice;
            return playerChoice;
        }
        private bool IsPlayerOne(String email)
        {
            return email == state.PlayerOne.Email;
        }

        private bool IsPlayerTwo(String email)
        {
            return email == state.PlayerTwo.Email;
        }


    }
}