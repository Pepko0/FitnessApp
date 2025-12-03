using FitnessApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FitnessApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        // GET: /login
        [HttpGet("login")]
        public IActionResult Login()
        {
            ViewBag.LoginMode = "operator";  

            if (HttpContext.Session.GetInt32("OperatorId") != null)
                return Redirect("/zarzadzanie");

            return View();
        }

        // POST: /login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var op = await _db.Operators
                .Include(o => o.Role)
                .FirstOrDefaultAsync(o => o.Email == email);

            if (op == null)
            {
                TempData["Error"] = "Nieprawidłowy email lub hasło.";
                return View();
            }

            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, op.Password);

            if (!passwordOk)
            {
                TempData["Error"] = "Nieprawidłowy email lub hasło.";
                return View();
            }

            HttpContext.Session.SetInt32("OperatorId", op.Id);
            HttpContext.Session.SetString("OperatorName", $"{op.FirstName} {op.LastName}");
            HttpContext.Session.SetString("OperatorRole", op.Role?.Name ?? "");
            HttpContext.Session.SetInt32("OperatorRoleId", op.RoleId);

            // WYMUSZENIE ZMIANY HASŁA
            if (op.ChangePassword)
                return RedirectToAction("ChangePasswordRequired"); // zrobimy ten widok w kroku D

            return Redirect("/zarzadzanie");
        }

        // GET: /logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }

        // ======= Forgot password =======

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var op = await _db.Operators
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (op == null)
            {
                ViewBag.Message = "Jeśli konto istnieje, wysłaliśmy wiadomość z resetem hasła.";
                return View(model);
            }

            var tempPassword = GenerateTempPassword();
            var token = GenerateToken();

            op.Password = BCrypt.Net.BCrypt.HashPassword(tempPassword);
            op.ChangePassword = true;
            op.PasswordResetToken = token;
            op.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

            await _db.SaveChangesAsync();

            var resetLink = Url.Action(
                "ResetPassword",
                "Auth",
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

            await _emailService.SendAsync(op.Email, "Reset hasła", body);

            ViewBag.Message = "Jeśli konto istnieje, wysłaliśmy wiadomość z resetem hasła.";
            return View(model);
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        // ======= Helpers =======

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

        // placeholder na krok D
        [HttpGet]
        public IActionResult ChangePasswordRequired()
        {
            return View();
        }

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var op = await _db.Operators
                .FirstOrDefaultAsync(x => x.PasswordResetToken == model.Token);

            if (op == null)
            {
                ViewBag.Error = "Nieprawidłowy lub zużyty token resetu.";
                return View(model);
            }

            if (op.PasswordResetTokenExpires == null || op.PasswordResetTokenExpires < DateTime.UtcNow)
            {
                ViewBag.Error = "Token resetu wygasł. Wygeneruj reset ponownie.";
                return View(model);
            }

            bool tempOk = BCrypt.Net.BCrypt.Verify(model.TempPassword, op.Password);
            if (!tempOk)
            {
                ViewBag.Error = "Hasło z maila jest niepoprawne.";
                return View(model);
            }

            op.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            op.ChangePassword = false;
            op.PasswordResetToken = null;
            op.PasswordResetTokenExpires = null;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Hasło zostało zmienione. Zaloguj się ponownie.";
            return RedirectToAction("Login");
        }
    }
}
