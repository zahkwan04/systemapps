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

            cartinit();

        }

        private void cartinit()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 1,2,3,4,2,4,5,5,4,3,2,3,4,4,2,3,55,5,5,5,5,5,3,4,5 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                }
            };

            Labels = new[] { "0:00", "1:00", "2:00", "3:00", "4:00", "5:00", "6:00", "7:00", "8:00", "9:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00","21:00", "22:00","23:00"};
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0 //straight lines, 1 really smooth lines
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[2].Values.Add(5d);

            DataContext = this;
        }

        private void cartinit(string[] lol)
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,7 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                }
            };

            Labels = lol;
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0 //straight lines, 1 really smooth lines
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[2].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SeriesCollection != null)
                {
                    SeriesCollection.Clear();


                }
                
                cartinit(new[] { "Jan22", "Feb22", "M22ar", "Ap22r", "Ma22y" });
                DataContext = Labels;
                DataContext = this;
                cartchart.Update(true, true); 
            }
            catch { }
        }
    }
}
