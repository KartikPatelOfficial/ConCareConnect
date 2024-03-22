using LaboratoryPanelWPF.ViewModels.TestMaster;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages.TestMaster
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : INavigableView<CategoriesViewModel>
    {
        public CategoriesViewModel ViewModel { get; }

        public CategoriesPage(CategoriesViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
