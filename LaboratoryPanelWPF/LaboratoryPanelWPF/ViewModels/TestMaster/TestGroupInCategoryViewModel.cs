using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.Service;
using LaboratoryPanelWPF.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LaboratoryPanelWPF.ViewModels.TestMaster
{
    public class TestGroupInCategoryViewModel : ViewModelBase
    {

        private readonly IDBService _dbService;
        private readonly Category _category;

        public TestGroupInCategoryViewModel(IDBService dbService, Category category)
        {
            _dbService = dbService;
            _category = category;

            TestGroups = new ObservableCollection<TestGroup>(_dbService.GetTestGroups(_category.Id));
        }

        public ObservableCollection<TestGroup> TestGroups { get; set; }

        public ICommand RunDeleteTestDialogCommand => new RelayCommand<TestGroup>(DeleteTestGroup);

        private void DeleteTestGroup(TestGroup group)
        {
            _dbService.RemoveTestGroupFromCategory(_category.Id, group.Id);
            TestGroups.Remove(group);
        }
    }
}
