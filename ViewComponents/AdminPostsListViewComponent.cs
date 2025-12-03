using FitnessApp.Data;
using FitnessApp.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.ViewComponents
{
    public class AdminPostsListViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public AdminPostsListViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roleId = HttpContext.Session.GetInt32("OperatorRoleId");

            bool isAdmin = roleId == 1;
            bool isTrainer = roleId == 3;
            bool isDietitian = roleId == 4;

            var query = _db.Posts
                .Include(p => p.Operator)
                .Include(p => p.Client)
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();

            if (!isAdmin)
            {
                if (isTrainer)
                    query = query.Where(p => p.Type == PostType.PersonalTraining);
                else if (isDietitian)
                    query = query.Where(p => p.Type == PostType.Diet);
                else
                    query = query.Where(p => false); // pracownik i inni -> nic
            }

            var posts = await query.ToListAsync();
            return View(posts);
        }
    }
}