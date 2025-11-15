using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessApp.Models.ViewModels
{
    public class EditOperatorViewModel
    {
        public Operator Operator { get; set; } = new Operator();
        public IEnumerable<OperatorRole> Roles { get; set; } = new List<OperatorRole>();

        public IEnumerable<SelectListItem> RoleItems =>
            Roles.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            });
    }
}