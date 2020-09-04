using AxAXVLC;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for vlcwpf.xaml
    /// </summary>
    public partial class vlcwpf : Window
    {

        bool conditionss = false;
        AxVLCPlugin2 vlc;
        public vlcwpf()
        {
            InitializeComponent();
            this.Topmost = true;
            vlc = new AxVLCPlugin2();
            winformhostvlc.Child = vlc;
            conditionss = false;






        }


        private void vlcscreenshot_Click(object sender, RoutedEventArgs e)
        {

            /* vlc.playlist.togglePause();
             System.Threading.Thread.Sleep(100);
             String tempPath = System.IO.Path.GetTempPath();
             string imgPath = tempPath + @"Captured.jpg";
             Bitmap bmpScreenshot = new Bitmap(vlc.ClientRectangle.Width,
                 vlc.ClientRectangle.Height);
             Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
             System.Drawing.Size imgSize = new System.Drawing.Size(
                 vlc.ClientRectangle.Width,
                 vlc.ClientRectangle.Height);
             System.Windows.Point ps = PointToScreen(new System.Windows.Point(vlc.Bounds.X, vlc.Bounds.Y));
             gfxScreenshot.CopyFromScreen((int)ps.X, (int)ps.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
             bmpScreenshot.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
             MessageBox.Show("Image saved : " + imgPath);
             File.WriteAllText(@"imagepath.txt", imgPath);
             vlc.playlist.togglePause();*/

            string root = "snapshotcam";

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            Directory.SetCurrentDirectory(root);
            vlc.playlist.togglePause();

            vlc.video.takeSnapshot();

            //MessageBox.Show("Screenshot succesful,location :" + root);
            vlc.playlist.play();


            File.WriteAllText(@"imagepath.txt", getlatestfile(Directory.GetCurrentDirectory()));

            Debug.WriteLine(Directory.GetCurrentDirectory().ToString());

            this.Close();



        }

        private void screenshotcam()
        {

            string root = "snapshotcam";

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            Directory.SetCurrentDirectory(root);
            vlc.playlist.togglePause();

            vlc.video.takeSnapshot();

            MessageBox.Show("Screenshot succesful,location :" + root);
            vlc.playlist.play();


            File.WriteAllText(@"imagepath.txt", getlatestfile(Directory.GetCurrentDirectory()));

            Debug.WriteLine(Directory.GetCurrentDirectory().ToString());
        }

        private void openandplaycam()
        {
            try
            {

                string camuri = File.ReadAllText("camurl.txt");
                var uri = new Uri(camuri);
                var convertedURI = uri.AbsoluteUri;
                vlc.playlist.add(convertedURI, " ", ":no-overlay");
                vlc.playlist.play();
                conditionss = true;


                // vlc.playlist.add(convertedURI);
                //vlc.playlist.play();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        private void AppsMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AppsPowerButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string getlatestfile(string dir)
        {
            string folder = dir;
            var files = new DirectoryInfo(folder).GetFiles("*.*");
            string latestfile = "";

            DateTime lastupdated = DateTime.MinValue;
            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastupdated)
                {
                    lastupdated = file.LastWriteTime;
                    latestfile = file.Name;
                }
            }
            return latestfile;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            openandplaycam();



        }


    }
}
