using System;
using System.ComponentModel.DataAnnotations;

namespace RPS.Api.PublicDomain
{
    public class MakeMoveCommand
    {
        [Required]
        public string Move { get; set; }

        [Required]
        public string PlayerName { get; set; }
    }
}