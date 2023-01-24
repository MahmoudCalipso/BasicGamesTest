using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicGamesEntities.Models
{
    public class Games
    {
        //public Games()
        //{
        //    Platforms =  new List<string>();
        //}
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? Game { get; set; }
        [Required]
        public int PlayTime { get; set; }
        [Required]
        public string Genre { get; set; }

        [Required]  
        public List<string> Platforms { get; set; } = new();
    }
}
