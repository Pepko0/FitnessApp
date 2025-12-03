using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Models.Enums
{
    public enum PostVisibility
    {
        [Display(Name = "Publiczny")]
        Public = 0,

        [Display(Name = "Tylko dla zalogowanych")]
        RegisteredOnly = 1,

        [Display(Name = "Prywatny")]
        Private = 2
    }
}
