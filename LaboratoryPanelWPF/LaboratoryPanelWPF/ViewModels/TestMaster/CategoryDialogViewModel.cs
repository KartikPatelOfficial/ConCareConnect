using System.ComponentModel.DataAnnotations;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class CategoryDialogViewModel : FormViewModel
    {
        private DialogMode _mode;


        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string PrintingName { get; set; } = string.Empty;

        [Required]
        public double Cost { get; set; }

        [Required]
        public double LabCharges { get; set; }

        [Required]
        public double Expenses { get; set; }


        public string OkButtonText { get => _mode == DialogMode.Add ? "Add" : "Edit"; }

        public string Title { get => _mode == DialogMode.Add ? "Add Category" : "Edit Category"; }

        public CategoryDialogViewModel(DialogMode mode)
        {
            _mode = mode;
        }

    }

}
