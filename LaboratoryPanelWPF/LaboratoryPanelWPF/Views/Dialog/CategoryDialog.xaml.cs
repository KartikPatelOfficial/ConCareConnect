using System.Windows;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Dialog
{
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : FluentWindow
    {
        public CategoryDialog()
        {
            InitializeComponent();
        }


        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
