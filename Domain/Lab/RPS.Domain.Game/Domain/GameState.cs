using System;
using RPS.Domain.Game.Events;

namespace RPS.Domain.Game.Domain
{
    public class GameState
    {
        public Guid Id { get; private set; }
        public string Title  { get; private set; }
        public string Winner  { get; private set; }
        public GamePlayer PlayerOne  { get; private set; }
        public GamePlayer PlayerTwo  { get; private set; }
        public int FirstTo  { get; private set; }
        public GameStatus State  { get; private set; }
        public int Round  { get; private set; }

        public void Handle(GameCreatedEvent @event)
        {
            Id = @event.GameId;
            Title = @event.Title;
            PlayerOne = new GamePlayer(@event.PlayerId);
            FirstTo = @event.FirstTo;
        }

        public void Handle(ChoiceMadeEvent @event)
        {
            if (IsPlayerOne(@event.PlayerId))
                PlayerOne.CurrentChoice = @event.Choice;
            else if (IsPlayerTwo(@event.PlayerId))
                PlayerTwo.CurrentChoice = @event.Choice;
        }

        public void Handle(RoundStartedEvent @event)
        {
            Round = @event.Round;
            PlayerOne.CurrentChoice = Choice.None;
            PlayerTwo.CurrentChoice = Choice.None;
        }

        public void Handle(RoundWonEvent @event)
        {
            if (IsPlayerOne(@event.PlayerId))
                PlayerOne.AddWin();
            else if (IsPlayerTwo(@event.PlayerId))
                PlayerTwo.AddWin();
        }

        public void Handle(GameWonEvent @event)
        {
            State = GameStatus.Finished;
            Winner = @event.PlayerId;
        }

        public void Handle(GameStartedEvent @event)
        {
            State = GameStatus.Started;
            PlayerTwo = new GamePlayer(@event.PlayerTwoId);
        }


        private bool IsPlayerOne(String email)
        {
            return email == PlayerOne.Email;
        }

        private bool IsPlayerTwo(String email)
        {
            return email == PlayerTwo.Email;
        }


    }
}