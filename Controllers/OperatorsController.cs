using FitnessApp.Models;
using FitnessApp.Services;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Helpers;

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
    }
}
