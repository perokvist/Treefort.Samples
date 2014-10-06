using System.ComponentModel.DataAnnotations;

namespace RPS.Game.ReadModel
{
    public class MakeMoveCommand
    {
        [Required]
        public string Move { get; set; }

        [Required]
        public string PlayerName { get; set; }
    }
}