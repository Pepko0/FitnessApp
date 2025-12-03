using System.ComponentModel.DataAnnotations;

public class RegisterClientViewModel
{
    [Required(ErrorMessage = "Imię jest wymagane")]
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy email")]
    [MaxLength(200)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć minimum 6 znaków")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare(nameof(Password), ErrorMessage = "Hasła nie są takie same")]
    public string ConfirmPassword { get; set; } = "";
}
