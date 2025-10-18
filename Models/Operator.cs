using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class Operator
    {
        public int Id { get; set; }

        /// <summary>
        /// Przechowuje imię operatora
        /// </summary>
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Przechowuje nazwisko operatora
        /// </summary>
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Przechowuje email operatora
        /// </summary>
        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Przechowuje hasło operatora
        /// </summary>
        [Required, MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Przechowuje role operatora na podstawie id z tabeli OepratorRoles
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Łączy tabelę Operators z OperatorRoles
        /// </summary>
        public OperatorRole? Role { get; set; }

        /// <summary>
        /// Przechowuje datę utworzenia rekordu
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Przechowuje datę aktualizacji rekordu
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
