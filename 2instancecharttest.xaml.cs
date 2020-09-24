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

            

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid sp = sender as Grid;
            DoubleAnimation db = new DoubleAnimation();
            //db.From = 12;
            db.To = 150;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            sp.BeginAnimation(Grid.HeightProperty, db);
            button.Height = 40;
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid sp = sender as Grid;
            DoubleAnimation db = new DoubleAnimation();
            //db.From = 12;
            db.To = 12;
            db.Duration = TimeSpan.FromSeconds(0.5);
            db.AutoReverse = false;
            db.RepeatBehavior = new RepeatBehavior(1);
            sp.BeginAnimation(Grid.HeightProperty, db);
            button.Height =300;
        }
    }
}

