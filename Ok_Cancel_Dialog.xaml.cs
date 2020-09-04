using System.Windows;
using System.Windows.Input;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for Ok_Cancel_Dialog.xaml
    /// </summary>
    public partial class Ok_Cancel_Dialog : Window
    {
        public int dialogOperation = 0;

        enum Operation
        {
            Cancel = 0,
            OK = 1
        }
        public Ok_Cancel_Dialog()
        {
            InitializeComponent();
        }

        private void OK_Cancel_OK_Click(object sender, RoutedEventArgs e)
        {
            dialogOperation = (int)Operation.OK;
            this.Close();
        }

        private void OK_Cancel_Cancel_Click(object sender, RoutedEventArgs e)
        {
            dialogOperation = (int)Operation.Cancel;
            this.Close();
        }

        private void Okcanceldialogbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
