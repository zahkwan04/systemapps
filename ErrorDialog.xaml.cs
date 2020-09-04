using System.Windows;
using System.Windows.Input;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : Window
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        private void ErrorDialog_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ErrorDialogBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
