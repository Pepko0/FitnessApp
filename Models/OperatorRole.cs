using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class OperatorRole
    {
        public int Id { get; set; }

        /// <summary>
        /// Przechowuje nazwę roli operatora
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
