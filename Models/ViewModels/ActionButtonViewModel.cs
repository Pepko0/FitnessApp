namespace FitnessApp.Models.ViewModels
{
    public class ActionButtonViewModel
    {
        public string Label { get; set; } = "";
        public string Url { get; set; } = "";
        public string CssClass { get; set; } = "btn btn-sm btn-primary";
        public string? Icon { get; set; } // np. "bi-pencil"
        public string? ModalTarget { get; set; } // np. "#deleteModal"
        public string? ItemName { get; set; }
        public int? ItemId { get; set; }
    }
}
