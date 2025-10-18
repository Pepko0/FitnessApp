using FitnessApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    public class OperatorRoleController : Controller
    {
        private readonly OperatorRoleService _roleService;

        public OperatorRoleController(OperatorRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("/zarzadzanie/operatorzy/role")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return View(roles);
        }

        [HttpPost("/zarzadzanie/operatorzy/role/dodaj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole([FromForm] string name)
        {
            var (success, message) = await _roleService.AddRoleAsync(name);

            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
