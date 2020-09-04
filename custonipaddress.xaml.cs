using System;
using System.Linq;
using System.Windows;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for custonipaddress.xaml
    /// </summary>
    public partial class custonipaddress : Window
    {
        bool checkipvalid;
        string ipaddresscustom;
        public custonipaddress()
        {
            InitializeComponent();
        }

        private void confirmbutton_Click(object sender, RoutedEventArgs e)
        {
            checkipvalid = IsValidIPv4(ipaddresstxtbox.Text);
            if (ipaddresstxtbox.Text == "" || checkipvalid == false)
            {
                MessageBox.Show("Please provide a valid ip address");
            }
            else
            {
                ipaddresscustom = ipaddresstxtbox.Text;
                this.Close();
            }
        }

        private void cancelbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public string returnvalue()
        {
            return ipaddresscustom;
        }

        private bool IsValidIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }


    }
}
