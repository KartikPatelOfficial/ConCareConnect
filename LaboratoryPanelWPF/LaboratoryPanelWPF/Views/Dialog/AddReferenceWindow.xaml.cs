using LaboratoryPanelWPF.Utils;
using LaboratoryPanelWPF.ViewModels;
using System.Windows;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Dialog
{
    /// <summary>
    /// Interaction logic for AddReferenceWindow.xaml
    /// </summary>
    public partial class AddReferenceWindow : FluentWindow
    {

        public AddReferenceWindow(ICallback callback)
        {
            DataContext = new AddReferenceViewModel(callback);
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
