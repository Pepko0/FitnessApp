using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Helpers
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Ustawia wiadomość sukcesu lub błędu w TempData i przekierowuje na wskazaną akcję.
        /// </summary>
        public static IActionResult RedirectWithMessage(this Controller controller, bool success, string message, string actionName = "Index")
        {
            if (success)
                controller.TempData["Success"] = message;
            else
                controller.TempData["Error"] = message;

            return controller.RedirectToAction(actionName);
        }
    }
}
