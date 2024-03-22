using LaboratoryPanelWPF.ViewModels;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages
{
    /// <summary>
    /// Interaction logic for AllAppointmentsPage.xaml
    /// </summary>
    public partial class AllAppointmentsPage : INavigableView<AllAppointmentsPageViewModel>
    {

        public AllAppointmentsPageViewModel ViewModel { get; }


        public AllAppointmentsPage(AllAppointmentsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

    }
}
