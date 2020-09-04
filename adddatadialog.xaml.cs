using System.Windows;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for adddatadialog.xaml
    /// </summary>
    public partial class adddatadialog : Window
    {

        public string name = "";
        public string description = "";
        public int dialogOperation = 0;

        enum Operation
        {
            Cancel = 0,
            OK = 1
        }
        public adddatadialog()
        {
            InitializeComponent();
        }

        private void newdataokbutton_Click(object sender, RoutedEventArgs e)
        {
            if (newdataname.Text != "")
            {
                name = newdataname.Text;
                description = newdatadescription.Text;
                dialogOperation = (int)Operation.OK;
            }
            else
            {
                dialogOperation = (int)Operation.Cancel;
            }
            this.Close();

        }

        private void newdatacancelbutton_Click(object sender, RoutedEventArgs e)
        {
            dialogOperation = (int)Operation.Cancel;
            this.Close();
        }
    }
}
