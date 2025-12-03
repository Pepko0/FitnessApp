using FitnessApp.Data;
using FitnessApp.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Controllers
{
    [Route("diety")]
    public class DietsController : Controller
    {
        private readonly AppDbContext _db;

        public DietsController(AppDbContext db)
        {
            _db = db;
        }

        // GET /diety
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts
                .Include(p => p.Operator)
                .Where(p => p.Type == PostType.Diet)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            var operatorId = HttpContext.Session.GetInt32("OperatorId");

            var post = await _db.Posts
                .Include(p => p.Operator)
                .FirstOrDefaultAsync(p => p.Id == id && p.Type == PostType.Diet);

            if (post == null)
                return NotFound();

            bool canSee =
                operatorId != null // operator widzi wszystko
                || post.Visibility == PostVisibility.Public
                || (post.Visibility == PostVisibility.RegisteredOnly && clientId != null)
                || (post.Visibility == PostVisibility.Private && clientId != null && post.ClientId == clientId);

            if (!canSee)
            {
                // jeśli gość i post niepubliczny -> przerzut na login
                if (clientId == null && operatorId == null && post.Visibility != PostVisibility.Public)
                {
                    var returnUrl = Url.Action("Details", "Diets", new { id = post.Id });
                    return Redirect($"/client/login?returnUrl={Uri.EscapeDataString(returnUrl!)}");
                }

                // jeśli zalogowany, ale nie ma dostępu (np. cudzy prywatny)
                return NotFound(); // albo RedirectToAction("Index") + TempData["Error"]
            }

            return View(post);
        }
    }
}
