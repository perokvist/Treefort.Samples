using Newtonsoft.Json.Linq;

namespace RPS.Api.Extensions
{
    public static class InputExtensions
    {
        public static Game.Move ToMove(this JObject self)
        {
            switch (self.Value<string>("move").ToLower())
            {
                case "paper":
                    return Game.Move.Paper;
                case "rock":
                    return Game.Move.Rock;
                case "scissors":
                    return Game.Move.Scissors;
                default :
                    return Game.Move.Paper;
            }

        }
    }
}