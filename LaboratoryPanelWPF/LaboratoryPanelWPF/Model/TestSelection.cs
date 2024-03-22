using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.ViewModels;

namespace LaboratoryPanel.Model
{
    internal class TestSelection : ViewModelBase

    {
        private bool isSelected = false;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public Category Category { get; }

        public TestSelection(Category category)
        {
            Category = category;
        }
    }
}