using Microsoft.AspNetCore.Mvc.Rendering;

namespace ScientificJournal.WebMVCApp.MinhPV.ViewModels
{
    public class AssignCategoriesViewModel
    {
        public int JournalId { get; set; }
        public string JournalName { get; set; } = string.Empty;
        public List<int> SelectedCategoryIds { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();
    }
}
