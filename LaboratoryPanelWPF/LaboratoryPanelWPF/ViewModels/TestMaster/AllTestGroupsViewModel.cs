using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Utils;
using LaboratoryPanelWPF.Views.Dialog;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class AllTestGroupsViewModel : ViewModelBase
    {

        private readonly IDBService _dbService;


        public ObservableCollection<TestGroup> TestGroups { get; }

        public ICommand RunDeleteCategoryDialogCommand { get; }
        public ICommand RunEditCategoryDialogCommand { get; }
        public ICommand RunAddTestGroupDialogCommand { get; }

        public AllTestGroupsViewModel(IDBService dbService)
        {
            RunDeleteCategoryDialogCommand = new RelayCommand<TestGroup>(RunDeleteCategoryDialog);
            RunEditCategoryDialogCommand = new RelayCommand<TestGroup>(RunEditCategoryDialog);
            RunAddTestGroupDialogCommand = new RelayCommand<Window>(RunAddTestGroupDialog);
            _dbService = dbService;
            TestGroups = new ObservableCollection<TestGroup>();

            //Load test groups in another thread
            Task.Run(() => LoadTestGroups());
        }

        private void RunAddTestGroupDialog(Window window)
        {
            var testGroupDialog = new TestGroupDialog();
            var testGroupViewModel = new TestGroupViewModel(DialogMode.Add);
            testGroupDialog.DataContext = testGroupViewModel;
            testGroupDialog.ShowDialog();

            if (testGroupDialog.DialogResult == true)
            {
                AddTestGroup(testGroupViewModel.Name, testGroupViewModel.PrintingName);
            }
        }

        private void RunEditCategoryDialog(TestGroup group)
        {
            var testGroupDialog = new TestGroupDialog();
            var testGroupViewModel = new TestGroupViewModel(DialogMode.Edit);
            testGroupViewModel.Name = group.GroupName;
            testGroupViewModel.PrintingName = group.PrintingName;
            testGroupDialog.DataContext = testGroupViewModel;
            testGroupDialog.ShowDialog();

            if (testGroupDialog.DialogResult == true)
            {
                group.GroupName = testGroupViewModel.Name;
                group.PrintingName = testGroupViewModel.PrintingName;

                _dbService.UpdateTestGroup(group.Id, group.GroupName, group.PrintingName);
            }
        }

        private async void LoadTestGroups()
        {
            var testGroups = await _dbService.GetTestGroups();

            //Clear the list before adding the new items in main thread
            Application.Current.Dispatcher.Invoke(() => TestGroups.Clear());


            foreach (var testGroup in testGroups)
            {
                Application.Current.Dispatcher.Invoke(() => TestGroups.Add(testGroup));
            }
        }

        private void RunDeleteCategoryDialog(TestGroup group)
        {
            var messageBox = MessageBox.Show($"Are you sure you want to delete {group.GroupName}?");
            if (messageBox == MessageBoxResult.OK)
            {
                _dbService.DeleteTestGroup(group.Id);
                TestGroups.Remove(group);
            }

        }

        public async void AddTestGroup(string name, string printingName)
        {
            var testGroup = await _dbService.AddTestGroup(name, printingName);
            TestGroups.Add(testGroup);
        }


    }
}
