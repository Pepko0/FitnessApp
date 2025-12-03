using FitnessApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;
using System.Security.Cryptography;
using System.Text;


namespace FitnessApp.Controllers
{
    
    [Route("client")]
    public class ClientAuthController : Controller
    {

        private readonly IEmailService _emailService;

        private readonly AppDbContext _db;

        public ClientAuthController(AppDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;

        }

        // GET: /client/login
        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.LoginMode = "client";

            if (HttpContext.Session.GetInt32("ClientId") != null)
                return Redirect(returnUrl ?? "/");

            ViewBag.ReturnUrl = returnUrl; // <— przekaż do widoku
            return View("~/Views/Auth/Login.cshtml");
        }

        // POST: /client/login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            ViewBag.LoginMode = "client";
            ViewBag.ReturnUrl = returnUrl;

            var client = await _db.Clients
                .FirstOrDefaultAsync(c => c.Email == email);

            if (client == null)
            {
                TempData["Error"] = "Nieprawidłowy email lub hasło.";
                return View("~/Views/Auth/Login.cshtml");
            }

            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, client.Password);
            if (!passwordOk)
            {
                TempData["Error"] = "Nieprawidłowy email lub hasło.";
                return View("~/Views/Auth/Login.cshtml");
            }

            HttpContext.Session.SetInt32("ClientId", client.Id);
            HttpContext.Session.SetString("ClientName", $"{client.FirstName} {client.LastName}");

            // BEZPIECZNY powrót tylko lokalny
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/");
        }

        // GET: /client/logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ClientId");
            HttpContext.Session.Remove("ClientName");
            return Redirect("/client/login");
        }

        [HttpGet("register")]
public IActionResult Register()
{
    return View(new RegisterClientViewModel());
}

        // POST: /client/register
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterClientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // sprawdzamy czy email już istnieje
            var exists = await _db.Clients.AnyAsync(c => c.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Email), "Konto z takim emailem już istnieje.");
                return View(model);
            }

            var client = new Client
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.Trim().ToLower(),

                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Type = 1, // domyślny typ klienta (zmienisz jak będziesz chciał)
                ChangePassword = false,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            // po rejestracji możesz od razu zalogować klienta:
            HttpContext.Session.SetInt32("ClientId", client.Id);
            HttpContext.Session.SetString("ClientName", $"{client.FirstName} {client.LastName}");

            return Redirect("/"); // strona główna
        }

        [HttpGet("forgot-password")]
public IActionResult ForgotPassword()
{
    return View(new ForgotPasswordViewModel());
}

[HttpPost("forgot-password")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    var client = await _db.Clients.FirstOrDefaultAsync(c => c.Email == model.Email);

    // nie zdradzamy czy istnieje
    if (client == null)
    {
        ViewBag.Message = "Jeśli konto istnieje, wysłaliśmy wiadomość z resetem hasła.";
        return View(model);
    }

    var tempPassword = GenerateTempPassword();
    var token = GenerateToken();

    client.Password = BCrypt.Net.BCrypt.HashPassword(tempPassword);
    client.ChangePassword = true;
    client.PasswordResetToken = token;
    client.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

    await _db.SaveChangesAsync();

    var resetLink = Url.Action(
        "ResetPassword",
        "ClientAuth",
        new { token = token },
        Request.Scheme
    );

    var body =
$@"Cześć,

Wygenerowaliśmy nowe hasło tymczasowe do Twojego konta:
{tempPassword}

Kliknij w link, aby ustawić nowe hasło:
{resetLink}

Link jest ważny 30 minut.

Pozdrawiamy,
Zespół FitnessApp";

    await _emailService.SendAsync(client.Email, "Reset hasła", body);

    ViewBag.Message = "Jeśli konto istnieje, wysłaliśmy wiadomość z resetem hasła.";
    return View(model);
}

[HttpGet("reset-password")]
public IActionResult ResetPassword(string token)
{
    return View(new ResetPasswordViewModel { Token = token });
}

[HttpPost("reset-password")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    var client = await _db.Clients
        .FirstOrDefaultAsync(c => c.PasswordResetToken == model.Token);

    if (client == null)
    {
        ViewBag.Error = "Nieprawidłowy lub zużyty token resetu.";
        return View(model);
    }

    if (client.PasswordResetTokenExpires == null || client.PasswordResetTokenExpires < DateTime.UtcNow)
    {
        ViewBag.Error = "Token resetu wygasł. Wygeneruj reset ponownie.";
        return View(model);
    }

    bool tempOk = BCrypt.Net.BCrypt.Verify(model.TempPassword, client.Password);
    if (!tempOk)
    {
        ViewBag.Error = "Hasło z maila jest niepoprawne.";
        return View(model);
    }

    client.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

    client.ChangePassword = false;
    client.PasswordResetToken = null;
    client.PasswordResetTokenExpires = null;

    await _db.SaveChangesAsync();

    TempData["Success"] = "Hasło zostało zmienione. Zaloguj się ponownie.";
    return RedirectToAction("Login");
}

// ======= Helpers (skopiuj z AuthController) =======

private static string GenerateTempPassword(int length = 10)
{
    const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$";
    var data = new byte[length];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(data);

    var sb = new StringBuilder(length);
    for (int i = 0; i < length; i++)
        sb.Append(chars[data[i] % chars.Length]);

    return sb.ToString();
}

private static string GenerateToken()
{
    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(48))
        .Replace("+", "-")
        .Replace("/", "_")
        .Replace("=", "");
}
    }
}
