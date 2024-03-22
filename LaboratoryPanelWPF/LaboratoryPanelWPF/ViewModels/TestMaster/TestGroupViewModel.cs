using System.ComponentModel.DataAnnotations;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class TestGroupViewModel : ViewModelBase
    {
        private DialogMode _dialogMode;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Printing name is required")]
        public string PrintingName { get; set; } = string.Empty;

        public string OkButtonText => _dialogMode == DialogMode.Add ? "Add" : "Edit";

        public TestGroupViewModel(DialogMode dialogMode)
        {
            _dialogMode = dialogMode;
        }

        public string Title => _dialogMode == DialogMode.Add ? "Add test group" : "Edit test group";

    }
}
