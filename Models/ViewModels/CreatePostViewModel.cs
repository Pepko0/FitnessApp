using System.ComponentModel.DataAnnotations;
using FitnessApp.Models.Enums;

public class CreatePostViewModel
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "";

    [Required]
    public string Content { get; set; } = "";

    // operator wybiera widoczność
    [Required]
    public PostVisibility Visibility { get; set; } = PostVisibility.Public;

    // opcjonalnie przypisanie do klienta
    public int? ClientId { get; set; }
}
