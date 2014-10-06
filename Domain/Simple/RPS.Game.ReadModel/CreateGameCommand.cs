using System.ComponentModel.DataAnnotations;

namespace RPS.Game.ReadModel
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