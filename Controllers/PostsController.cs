using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Controllers
{
    [Route("zarzadzanie/posts")]
    public class PostsController : Controller
    {
        private readonly AppDbContext _db;

        public PostsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /zarzadzanie/posts
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(); // bez modelu
        }

        [HttpGet("dodaj")]
        public async Task<IActionResult> Create()
        {
            if (!HasRole(1, 4)) 
                return Forbid();
        
            ViewBag.Clients = await _db.Clients
                .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                .ToListAsync();

            return View(new CreatePostViewModel());
        }


        // POST: /zarzadzanie/posts/dodaj
        [HttpPost("dodaj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!HasRole(1, 4))
                return Forbid();
        
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = await _db.Clients
                    .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    .ToListAsync();

                return View(model);
            }

            var operatorId = HttpContext.Session.GetInt32("OperatorId");
            if (operatorId == null)
                return Redirect("/login");

            // WALIDACJA VISIBILITY
            if (model.Visibility == PostVisibility.Private && model.ClientId == null)
            {
                ModelState.AddModelError(nameof(model.ClientId),
                    "Dla widoczności prywatnej musisz wskazać klienta.");

                ViewBag.Clients = await _db.Clients
                    .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    .ToListAsync();

                return View(model);
            }

            var post = new Post
            {
                Title = model.Title.Trim(),
                Content = model.Content.Trim(),
                Visibility = model.Visibility,
                ClientId = model.ClientId,

                // TU jest sztywno dieta:
                Type = PostType.Diet,

                OperatorId = operatorId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Post dietetyczny został dodany.";
            return RedirectToAction("Index");
        }

        // GET: /zarzadzanie/posts/dodaj-trening   (trenerzy)
        [HttpGet("dodaj-trening")]
        public async Task<IActionResult> CreateTraining()
        {
            if (!HasRole(1, 3)) 
                return Forbid();

            ViewBag.Clients = await _db.Clients
                .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                .ToListAsync();

            return View("CreateTraining", new CreatePostViewModel());
        }


        // POST: /zarzadzanie/posts/dodaj-trening   (trenerzy)
        [HttpPost("dodaj-trening")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTraining(CreatePostViewModel model)
        {
            if (!HasRole(1, 3))
                return Forbid();
        
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = await _db.Clients
                    .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    .ToListAsync();

                return View("CreateTraining", model);
            }

            var operatorId = HttpContext.Session.GetInt32("OperatorId");
            if (operatorId == null)
                return Redirect("/login");

            if (model.Visibility == PostVisibility.Private && model.ClientId == null)
            {
                ModelState.AddModelError(nameof(model.ClientId),
                    "Dla widoczności prywatnej musisz wskazać klienta.");

                ViewBag.Clients = await _db.Clients
                    .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    .ToListAsync();

                return View("CreateTraining", model);
            }

            var post = new Post
            {
                Title = model.Title.Trim(),
                Content = model.Content.Trim(),
                Visibility = model.Visibility,
                ClientId = model.ClientId,

                // TU sztywno trening personalny:
                Type = PostType.PersonalTraining,

                OperatorId = operatorId.Value,
                CreatedAt = DateTime.UtcNow
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Post treningowy został dodany.";
            return RedirectToAction("Index");
        }

        private bool HasRole(params int[] allowedRoleIds)
        {
            var roleId = HttpContext.Session.GetInt32("OperatorRoleId");
            return roleId != null && allowedRoleIds.Contains(roleId.Value);
        }

    }
}
