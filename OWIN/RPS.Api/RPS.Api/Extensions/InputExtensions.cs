using Newtonsoft.Json.Linq;

namespace RPS.Api.Extensions
{
    public static class InputExtensions
    {
        public static Common.Move ToMove(this JObject self)
        {
            switch (self.Value<string>("move").ToLower())
            {
                case "paper":
                    return Common.Move.Paper;
                case "rock":
                    return Common.Move.Rock;
                case "scissors":
                    return Common.Move.Scissors;
                default :
                    return Common.Move.Paper;
            }

        }
    }
}