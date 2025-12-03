using System;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Models.Enums;

namespace FitnessApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Required]
        public string Content { get; set; } = "";

        // kto dodał (operator)
        public int OperatorId { get; set; }
        public Operator Operator { get; set; } = null!;

        // dla kogo (opcjonalnie)
        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public PostType Type { get; set; }
        public PostVisibility Visibility { get; set; } = PostVisibility.Public;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
