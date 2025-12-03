using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";   // tutaj trzymasz HASH (BCrypt)

        public int Type { get; set; }                // na razie int, później można zrobić enum

        // reset hasła / wymuszenie zmiany
        public bool ChangePassword { get; set; } = false;
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
