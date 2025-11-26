using FitnessApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FitnessApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /login
        [HttpGet("login")]
        public IActionResult Login()
        {
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

            // poprawne porównanie BCrypt
            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, op.Password);

            if (!passwordOk)
            {
                TempData["Error"] = "Nieprawidłowy email lub hasło.";
                return View();
            }

            // zapis do sesji
            HttpContext.Session.SetInt32("OperatorId", op.Id);
            HttpContext.Session.SetString("OperatorName", $"{op.FirstName} {op.LastName}");
            HttpContext.Session.SetString("OperatorRole", op.Role?.Name ?? "");

            return Redirect("/zarzadzanie");
        }

        // GET: /logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }
    }
}
