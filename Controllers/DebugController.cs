using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


/**
* Służy dod dodawania roli oraz admina w momencie kiedy nie ma danych w bazie danych
*/


namespace FitnessApp.Controllers
{
    [Route("debug")]
    public class DebugController : Controller
    {
        private readonly AppDbContext _db;

        public DebugController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("create-admin")]
        public async Task<IActionResult> CreateAdmin()
        {
            if (await _db.Operators.AnyAsync())
                return Content("Operators already exist!");

            var admin = new Operator
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.pl",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                RoleId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Operators.Add(admin);
            await _db.SaveChangesAsync();

            return Content("Admin created!");
        }

        [HttpGet("debug/add-roles")]
        public IActionResult AddRoles([FromServices] AppDbContext db)
        {
            db.OperatorRoles.AddRange(
                new OperatorRole { Name = "Admin" },
                new OperatorRole { Name = "Pracownik" }
            );
        
            db.SaveChanges();
        
            return Ok("Dodano role");
        }
    }
}
