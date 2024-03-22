using CommunityToolkit.Mvvm.Input;
using LaboratoryPanelWPF.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace LaboratoryPanelWPF.ViewModels
{
    public class LoginWindowViewModel : FormViewModel
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        private string? _email;
        private string? _password;


        public RelayCommand LoginCommand { get; set; }

        public LoginWindowViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            LoginCommand = new RelayCommand(Login);
            _authService = authService;
            _serviceProvider = serviceProvider;
        }


        [Required, EmailAddress]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));

            }
        }

        [Required, MinLength(6)]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));

            }
        }


        private async void Login()
        {
            if (HasErrors) return;

            try
            {
                await _authService.Login(Email, Password);
                var mainWindow = _serviceProvider.GetService(typeof(MainWindow)) as MainWindow;
                mainWindow?.Show();
                Application.Current.MainWindow?.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
