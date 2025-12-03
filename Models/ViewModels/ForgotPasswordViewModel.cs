using System.ComponentModel.DataAnnotations;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format emaila")]
    public string Email { get; set; } = "";
}
