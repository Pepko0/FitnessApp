using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    [Required]
    public string Token { get; set; } = "";

    [Required(ErrorMessage = "Hasło z maila jest wymagane")]
    public string TempPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć minimum 6 znaków")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare(nameof(NewPassword), ErrorMessage = "Hasła nie są takie same")]
    public string ConfirmNewPassword { get; set; } = "";
}
