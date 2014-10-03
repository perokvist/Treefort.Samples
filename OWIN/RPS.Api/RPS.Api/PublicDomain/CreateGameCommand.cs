using System.ComponentModel.DataAnnotations;

namespace RPS.Api.PublicDomain
{
    public class CreateGameCommand
    {
        [Required]
        public string PlayerName { get; set; }

        [Required]
        public string GameName { get; set; }
        
        [Required]
        public string Move { get; set; }
    }
}