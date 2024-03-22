using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Utils;
using LaboratoryPanelWPF.Views.Dialog;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class TestGroupsViewModel : ViewModelBase
    {

        private readonly IDBService _dbService;
        public Category? Category { get; set; }


        public ICommand RunEditCategoryDialogCommand { get; set; }
        public ICommand RunAddCategoryDialogCommand { get; set; }
        public ICommand RunDeleteCategoryDialogCommand { get; set; }

        public TestGroupsViewModel(IDBService dbService)
        {
            _dbService = dbService;


            RunEditCategoryDialogCommand = new RelayCommand<TestGroup>(EditCategory);
            RunAddCategoryDialogCommand = new RelayCommand<Window>(AddCategory);
            RunDeleteCategoryDialogCommand = new RelayCommand<TestGroup>(DeleteCategory);
        }

        public void LoadData()
        {
            if (Category == null) return;
            TestGroups = new ObservableCollection<TestGroup>(_dbService.GetTestGroups(Category.Id));
        }


        private void DeleteCategory(TestGroup group)
        {
            var result = MessageBox.Show("Are you sure you want to delete this test group?", "Delete test group", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            _dbService.DeleteTestGroup(group.Id);
            TestGroups.Remove(group);
        }

        private async void AddCategory(Window window)
        {

            var dialog = new TestGroupDialog();
            dialog.DataContext = new TestGroupViewModel(DialogMode.Add);
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var viewModel = dialog.DataContext as TestGroupViewModel;
                var group = await _dbService.AddTestGroup(viewModel!.Name, viewModel.PrintingName);
                TestGroups.Add(group);
            }
        }

        private void EditCategory(TestGroup group)
        {
            var dialog = new TestGroupDialog();
            dialog.DataContext = new TestGroupViewModel(DialogMode.Edit)
            {
                Name = group.GroupName,
                PrintingName = group.PrintingName
            };
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var viewModel = dialog.DataContext as TestGroupViewModel;
                var name = viewModel!.Name;
                var printingName = viewModel.PrintingName;

                _dbService.UpdateProperty("TestGroups", "GroupName", name, "Id", group.Id);
                _dbService.UpdateProperty("TestGroups", "PrintingName", printingName, "Id", group.Id);

                group.GroupName = viewModel.Name;
                group.PrintingName = viewModel.PrintingName;
            }
        }

        public ObservableCollection<TestGroup> TestGroups { get; set; }
    }
}
