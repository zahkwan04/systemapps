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

        public datatemplatetest()
        {
            InitializeComponent();

           
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

     
    }

  
}


/*  <ContentControl >
       <ContentControl.ContentTemplate>
           <DataTemplate>
               <Grid>
                   <TextBlock Text="{Binding Firstname}" FontSize="15" Margin="427,134,43.6,253"/>
               </Grid>
           </DataTemplate>
       </ContentControl.ContentTemplate>
   </ContentControl>*/
