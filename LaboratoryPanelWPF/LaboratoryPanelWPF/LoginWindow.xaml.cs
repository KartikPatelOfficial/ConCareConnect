using LaboratoryPanelWPF.ViewModels;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginWindow : FluentWindow
    {
        public LoginWindow(LoginWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginWindowViewModel)DataContext).Password = LoginPassword.Password;
        }

        private void CloseButton_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
