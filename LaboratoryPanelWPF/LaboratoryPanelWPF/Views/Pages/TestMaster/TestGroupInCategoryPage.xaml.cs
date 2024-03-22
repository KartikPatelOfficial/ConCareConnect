using System.Windows.Controls;
using LaboratoryPanelWPF.ViewModels.TestMaster;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages.TestMaster
{
    /// <summary>
    /// Interaction logic for TestGroupInCategoryPage.xaml
    /// </summary>
    public partial class TestGroupInCategoryPage : INavigableView<TestGroupInCategoryViewModel>
    {
        public TestGroupInCategoryViewModel ViewModel { get; }
        public TestGroupInCategoryPage(TestGroupInCategoryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
