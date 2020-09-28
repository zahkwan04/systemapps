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
using LiveCharts.Defaults;
using System.Windows.Media.Animation;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for _2instancecharttest.xaml
    /// </summary>
    public partial class _2instancecharttest : Window
    {
      
        public _2instancecharttest()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Vechicle 1",
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true,
                    
                    
                },
                new StackedColumnSeries
                {
                    Title = "Vechicle 2",
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true,
                }
            };

           

            Labels = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            Formatter = value => value + "";

            DataContext = this;

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


    }
}

