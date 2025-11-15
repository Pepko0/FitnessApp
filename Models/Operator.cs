using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class Operator
    {
        public int Id { get; set; }
    
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
    
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
    
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty;
    
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    
        public int RoleId { get; set; }
        public OperatorRole? Role { get; set; }
    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
