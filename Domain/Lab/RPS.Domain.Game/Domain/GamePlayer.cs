namespace RPS.Domain.Game.Domain
{
    public class GamePlayer
    {
        public GamePlayer(string email)
        {
            Email = email;
        }

        public string Email { get; set; }

        public Choice CurrentChoice { get; set; }

        public decimal Score { get; set; }

        public void AddWin()
        {
            Score += 1;
        }
    }
}
