using LaboratoryPanelWPF.ViewModels.TestMaster;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages.TestMaster
{
    /// <summary>
    /// Interaction logic for AllTestGroupsPage.xaml
    /// </summary>
    public partial class AllTestGroupsPage : INavigableView<AllTestGroupsViewModel>
    {
        public AllTestGroupsViewModel ViewModel { get; }

        public AllTestGroupsPage(AllTestGroupsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
