using LaboratoryPanelWPF.ViewModels.TestMaster;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages.TestMaster
{
    /// <summary>
    /// Interaction logic for TestGroupsPage.xaml
    /// </summary>
    public partial class TestGroupsPage : INavigableView<TestGroupsViewModel>
    {
        public TestGroupsViewModel ViewModel { get; set; }
        private readonly INavigationService _navigationService;

        public TestGroupsPage(TestGroupsViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
            _navigationService = navigationService;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.GetNavigationControl().Transition = Wpf.Ui.Animations.Transition.SlideBottom;
            _navigationService.Navigate(typeof(CategoriesPage));
            _navigationService.GetNavigationControl().Transition = Wpf.Ui.Animations.Transition.FadeIn;
        }
    }
}
