using System.Windows;
using Wpf.Ui.Controls;

namespace LaboratoryPanelWPF.Views.Dialog
{
    /// <summary>
    /// Interaction logic for TestGroupDialog.xaml
    /// </summary>
    public partial class TestGroupDialog : FluentWindow
    {
        public TestGroupDialog()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
