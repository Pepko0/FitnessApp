namespace FitnessApp.Models.ViewModels
{
    public class TableColumn
    {
        public string Header { get; set; } = "";
        public Func<object, object?>? ValueSelector { get; set; }
    }

    public class TableAction
    {
        public Func<object, string>? UrlSelector { get; set; }
        public Func<object, string>? LabelSelector { get; set; }
        public string CssClass { get; set; } = "btn btn-sm btn-primary";
        public string? Icon { get; set; }
        public bool IsModal { get; set; } = false;
        public string? ModalId { get; set; }
        public Func<object, string>? NameSelector { get; set; }
    }

    public class TableViewModel
    {
        public IEnumerable<object> Items { get; set; } = Array.Empty<object>();
        public List<TableColumn> Columns { get; set; } = new();
        public List<TableAction>? Actions { get; set; }

        public string? ModalPartial { get; set; }
        public object? ModalModel { get; set; }
    }
}
