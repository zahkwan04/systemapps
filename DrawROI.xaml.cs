using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using Sysdraw = System.Drawing;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for DrawROI.xaml
    /// </summary>
    public partial class DrawROI : Window
    {
        Sysdraw.Bitmap newimage = null;
        int positiveroi = 0;
        int refWidth;
        int refHeight;
        int oriWidth;
        int oriHeight;
        int clickcount = 0;
        int noofroidrawn = 0;
        Point[][] drawnroi = new Point[20][];
        public static Point[] polycoorobj = null;
        Boolean falseroi = false;
        int overlaproi = 1;
        Boolean specialroi = false;
        int[] roitype = new int[6];
        public DrawROI()
        {
            InitializeComponent();
        }

        private void DrawRoiBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Confirm_ROI_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void drawroiselectfilebutton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string thumb = dlg.FileName;
                Sysdraw.Image oriimage = Sysdraw.Image.FromFile(thumb);
                Sysdraw.Bitmap newimage = new Sysdraw.Bitmap(oriimage, 352, 288);

                if (oriimage.Width <= 800 && oriimage.Height <= 600)
                {
                    oriWidth = oriimage.Width;
                    oriHeight = oriimage.Height;


                }
                else
                {
                    int newWidth = oriimage.Width / 2;
                    int newHeight = oriimage.Height / 2;

                    refWidth = newWidth;
                    refHeight = newHeight;
                }


            }

        }

    }

}
