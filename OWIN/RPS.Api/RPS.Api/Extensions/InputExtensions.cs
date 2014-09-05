using Newtonsoft.Json.Linq;
using RPS.Game.Domain;

namespace RPS.Api.Extensions
{
    public static class InputExtensions
    {
        public static Move ToMove(this JObject self)
        {
            switch (self.Value<string>("move").ToLower())
            {
                case "paper":
                    return Move.Paper;
                case "rock":
                    return Move.Rock;
                case "scissors":
                    return Move.Scissors;
                default :
                    return Move.Paper;
            }

        }
    }
}