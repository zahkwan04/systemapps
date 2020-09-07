using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for datatemplatetest.xaml
    /// </summary>
    public partial class datatemplatetest : Window
    {

        public datatemplatetesttt PersonTest { get; set; }
        public datatemplatetest()
        {
            InitializeComponent();

            PersonTest = new datatemplatetesttt();
            PersonTest.Gender = Gender.Male;
            PersonTest.Age = 24;
            PersonTest.Firstname = "Razak";
            PersonTest.lastname = "Karim";

            this.DataContext = this;
        }
    }

  
}
