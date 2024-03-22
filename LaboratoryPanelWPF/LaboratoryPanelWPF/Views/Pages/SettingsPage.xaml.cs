using LaboratoryPanelWPF.ViewModels;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Pages;

public partial class SettingsPage : INavigableView<SettingsPageViewModel>
{
    public SettingsPageViewModel ViewModel { get; }
    
    public SettingsPage(SettingsPageViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

}