
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;
 using CommunityToolkit.Mvvm.ComponentModel;


 namespace LaboratoryPanelWPF.ViewModels;


public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "ConCare Connect";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Home",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(Views.Pages.HomePage)
        },
        new NavigationViewItem()
        {
            Content = "Appointments",
            Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
            TargetPageType = typeof(Views.Pages.AllAppointmentsPage)
        },
        new NavigationViewItem()
        {
            Content = "Categories",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Folder24 },
            TargetPageType = typeof(Views.Pages.TestMaster.CategoriesPage)
        },
        new NavigationViewItem()
        {
            Content = "Test Groups",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Folder24 },
            TargetPageType = typeof(Views.Pages.TestMaster.AllTestGroupsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Settings",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(Views.Pages.SettingsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new()
    {
        new MenuItem { Header = "Home", Tag = "tray_home" }
    };
}
