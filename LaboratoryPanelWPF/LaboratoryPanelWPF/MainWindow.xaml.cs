using LaboratoryPanelWPF.ViewModels;
using System;
using Wpf.Ui;

namespace LaboratoryPanelWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    { public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);
        }
    }
}