using FitnessApp.Models;
using FitnessApp.Services;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Helpers;
using FitnessApp.Models.ViewModels;



namespace FitnessApp.Controllers
{
    [Route("/zarzadzanie/operatorzy")]
    public class OperatorsController : Controller
    {
        private readonly OperatorService _operatorService;

        public OperatorsController(OperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        // GET: /zarzadzanie/operatorzy
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var vm = await _operatorService.GetOperatorsViewModelAsync();
            return View(vm);
        }

        [HttpPost("dodaj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOperator([FromForm] Operator op)
        {
            var (success, message) = await _operatorService.AddOperatorAsync(op);
            return this.RedirectWithMessage(success, message);

        }

        [HttpGet("edytuj/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var op = await _operatorService.GetOperatorByIdAsync(id);

            if (op == null)
                return NotFound();

            var roles = await _operatorService.GetAllRolesAsync();
            Console.WriteLine("ROLES LOADED = " + roles.Count());

            var vm = new EditOperatorViewModel
            {
                Operator = op,
                Roles = roles
            };

            return View(vm); 
        }

        [HttpPost("edytuj/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditOperatorViewModel vm)
        {
            Console.WriteLine("UPDATED ROLE ID = " + vm.Operator.RoleId);

            var (success, message) = await _operatorService.UpdateOperatorAsync(id, vm.Operator);
            return this.RedirectWithMessage(success, message);
        }

        [HttpPost("usun/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _operatorService.DeleteOperatorAsync(id);
            return this.RedirectWithMessage(success, message);
        }
    }
}
