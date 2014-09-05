namespace RPS.Game.Domain
{
   public enum Move
   {
       Rock, Paper, Scissors
   }

    public enum GameResult
    {
        PlayerOneWin, PlayerTwoWin, Tie
    }

    public enum GameStatus
    {
        NotStarted, Created, Started, Ended
    }
}