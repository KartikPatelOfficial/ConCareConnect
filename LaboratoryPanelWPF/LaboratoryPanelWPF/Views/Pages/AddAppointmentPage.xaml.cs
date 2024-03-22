using LaboratoryPanelWPF.Model;
using LaboratoryPanelWPF.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for AddAppointmentPage.xaml
    /// </summary>
    public partial class AddAppointmentPage : INavigableView<AddAppointmentViewModel>
    {
        public AddAppointmentPage(AddAppointmentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        public AddAppointmentViewModel ViewModel { get; private set; }

        private void OnTestSelectionChange(object sender, SelectionChangedEventArgs e)
        {

            if (AllTestsDataGrid.SelectedItem != null)
            {
                var selectedTest = AllTestsDataGrid.SelectedItem as Category;

                ViewModel.AddTest(selectedTest);
            }
        }
    }
}
