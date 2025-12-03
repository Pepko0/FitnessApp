using FitnessApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.ViewComponents
{
    public class AdminDashboardTilesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var roleId = HttpContext.Session.GetInt32("OperatorRoleId");

            bool isAdmin = roleId == 1;
            bool isTrainer = roleId == 3;
            bool isDietitian = roleId == 4;

            var tiles = new List<DashboardTileVM>();

            // Lista artykułów: admin + trener + dietetyk
            if (isAdmin || isTrainer || isDietitian)
            {
                tiles.Add(new DashboardTileVM
                {
                    Title = "Lista artykułów",
                    Subtitle = "Diety i treningi",
                    Url = "/zarzadzanie/posts",
                    IconClass = "bi bi-journals"
                });
            }

            // Dodaj dietę: admin + dietetyk
            if (isAdmin || isDietitian)
            {
                tiles.Add(new DashboardTileVM
                {
                    Title = "Dodaj nową dietę",
                    Subtitle = "Wpis dietetyczny",
                    Url = "/zarzadzanie/posts/dodaj",
                    IconClass = "bi bi-egg-fried"
                });
            }

            // Dodaj trening: admin + trener
            if (isAdmin || isTrainer)
            {
                tiles.Add(new DashboardTileVM
                {
                    Title = "Dodaj trening personalny",
                    Subtitle = "Wpis treningowy",
                    Url = "/zarzadzanie/posts/dodaj-trening",
                    IconClass = "bi bi-activity"
                });
            }

            // Pracownicy + Role: tylko admin
            if (isAdmin)
            {
                tiles.Add(new DashboardTileVM
                {
                    Title = "Pracownicy",
                    Subtitle = "Zarządzanie kontami",
                    Url = "/zarzadzanie/operatorzy",
                    IconClass = "bi bi-people"
                });

                tiles.Add(new DashboardTileVM
                {
                    Title = "Role",
                    Subtitle = "Uprawnienia i dostęp",
                    Url = "/zarzadzanie/operatorzy/role",
                    IconClass = "bi bi-shield-lock"
                });
            }

            return View(tiles);
        }
    }
}
