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
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for _2instancecharttest.xaml
    /// </summary>
    public partial class _2instancecharttest : Window
    {
        int counter;
        private double _value;
        DispatcherTimer dt = new DispatcherTimer();
        public _2instancecharttest()
        {
            InitializeComponent();
            

            dt.Interval = TimeSpan.FromSeconds(0.25);
            dt.Tick += Dt_Tick;
            dt.Start();

            DataContext = this;
        }

        private void Dt_Tick(object sender, EventArgs e)
        {

            if (counter <= 100)
            {
                counter++;
               gauge1.Value = counter;
                textblock1.Text = counter.ToString();
            }
            else
            {
                counter = 0;
               gauge1.Value = counter;
                textblock1.Text = counter.ToString();

            }

        }

        





    }
}
