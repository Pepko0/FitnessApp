using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models
{
    public class OperatorRole
    {
        public int Id { get; set; }

        /// <summary>
        /// Przechowuje nazwę roli operatora
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;    }
}
