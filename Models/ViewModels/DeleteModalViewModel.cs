namespace FitnessApp.Models.ViewModels
{
    public class DeleteModalViewModel
    {
        public string ModalId { get; set; } = "";
        public string FormId { get; set; } = "";
        public string ItemNameId { get; set; } = "";

        public string Title { get; set; } = "Usuń element";
        public string QuestionText { get; set; } = "Czy na pewno chcesz usunąć:";

        public string DeleteUrlBase { get; set; } = "";
    }
}
