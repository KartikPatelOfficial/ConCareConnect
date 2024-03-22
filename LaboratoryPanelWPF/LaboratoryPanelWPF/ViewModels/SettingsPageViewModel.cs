using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LaboratoryPanelWPF.Service;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.ViewModels;

public partial class SettingsPageViewModel : ObservableObject, INavigationAware
{

    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private Wpf.Ui.Appearance.ApplicationTheme _currentTheme = Wpf.Ui.Appearance.ApplicationTheme.Unknown;

    private IAuthService _authService;

    public SettingsPageViewModel(IAuthService authService)
    {
        _authService = authService;

        LogoutCommand = new RelayCommand(Logout);
    }

    public RelayCommand LogoutCommand { get; set; }

    public void Logout()
    {
        _authService.Logout();
    }

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
        CurrentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
        AppVersion = $"DS Laboratory - {GetAssemblyVersion()}";

        _isInitialized = true;
    }

    private string GetAssemblyVersion()
    {
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
               ?? String.Empty;
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private void OnChangeTheme(string parameter)
    {
        switch (parameter)
        {
            case "theme_light":
                if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Light)
                    break;

                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light);
                CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Light;

                break;

            default:
                if (CurrentTheme == Wpf.Ui.Appearance.ApplicationTheme.Dark)
                    break;

                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Dark);
                CurrentTheme = Wpf.Ui.Appearance.ApplicationTheme.Dark;

                break;
        }
    }
}
