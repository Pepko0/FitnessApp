using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    [Route("zarzadzanie")]
    public class AdministrationController : Controller
    {
        // GET /zarzadzanie
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
