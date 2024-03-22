using DiagnosticServicesModel.DataClass;
using LaboratoryPanelWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for PatientPage.xaml
    /// </summary>
    public partial class PatientPage : INavigableView<PatientViewModel>
    {

        public PatientViewModel ViewModel { get; private set; }

        private readonly INavigationService _navigationService;

        public PatientPage(PatientViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();

            _navigationService = navigationService;
        }

        private void OnClickBackButton(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void PatientSelected(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridPatients.SelectedItem is not Patient selectedPatient)
            {
                return;
            }
            AddAppointmentViewModel.Patient = selectedPatient;
            _navigationService.NavigateWithHierarchy(typeof(AddAppointmentPage));
        }
    }
}
