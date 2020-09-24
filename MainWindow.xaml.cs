using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using systemapps.DBops;
using systemapps.Properties;
using Winforms = System.Windows.Forms;
using Microsoft.Win32;



namespace systemapps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region  //variables declaration region
        string currentscriptname;
        string scriptpath;
       

        string imagefolderdir;
        string vidfolderdir;
        int counterimg;
        int countervid;
        List<string> imgfilepath = new List<string>();
        List<string> vidfilepath = new List<string>();

        int totalvehiclemin;
        double totalvehicleavg;
        int totalvehiclemax;

        double speedmin;
        double speedavg;
        double speedmax;

        double gapmin;
        double gapavg;
        double gapmax;

        int[] vehicledistributionarray = new int[6] ;
        List<int> vehicledislist = new List<int>();


        string urlfoldercam;

        string[] predmodelitems = new string[7];
        string[] detmodelitems = new string[7];
        string[] camitems = new string[5];

        double[] arrayspeedavg = new double[] {};
        double[] arraygapavg = new double[] {};
        string[] timespeedstatsarray = new string[] {};
 
        string detdebug;
        string trkdebug;
        string predenable;
        
        private bool isConnectionTested = false;
        enum Operation
        {
            Cancel = 0,
            OK = 1
        }

        DispatcherTimer dt = new DispatcherTimer();
        DispatcherTimer dt2 = new DispatcherTimer();


        #endregion 
        //end region for variables declaration

        public MainWindow()
        {

            InitializeComponent();
            init();
            piechartinit();
            
            dt.Interval = TimeSpan.FromSeconds(900);
            dt.Tick += Dt_Tick;


            }

        private void init()
        {

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            datesearchfilter.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
            timerangefrom.Text = "0:00";
            timerangeto.Text = "23:00";
            cbbanalyticfiltersearch.SelectedIndex = 1;

            Dbipaddresstestconn.Text = Settings.Default.dbipaddresstestconn;
            Dbnametestconn.Text = Settings.Default.dbnametestconn;
            Dbusernametestconn.Text = Settings.Default.dbusernametestconn;
            Dbpasswordtestconn.Password = Settings.Default.dbpasswordtestconn;
            imagefolderdir = null;
            counterimg = 0;
            string date = DateTime.UtcNow.ToString("dd-MM-yyyy");
            string time = DateTime.Now.ToString("HH:mm:ss");
            Debug.WriteLine(time + " " + date);
            totalvehiclestatstxtbox.Text = "0";
            vehi1stats.Text = "0";
            vehi2stats.Text = "0";
            vehi3stats.Text = "0";
            vehi4stats.Text = "0";
            vehi5stats.Text = "0";
            vehi6stats.Text = "0";
            avgspeedstatstxtbox.Text = "0";
            avggapstatstxtbox.Text = "0";

            biggesttabinthisapps.SelectedIndex = 1;

        }


        #region //cartesian chart for GAP

        private void cartchartinit()
        {

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                   Title = "Gap",
                    LineSmoothness = 1,
                    Values = null,
                    Stroke = Brushes.DeepSkyBlue

                },
              
            };

            Labels = timespeedstatsarray;
            YFormatter = value => value.ToString("");

      

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        #endregion
        //end region for cartesian chart for GAP



        #region //cartesian chart for SPEED
        private void cartinit2()
        {
            SeriesCollection2 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Speed",
                    Stroke = Brushes.DarkBlue,                      
                    Values = null
                },
           
            };

            Labels2 = timespeedstatsarray;
            YFormatter2 = value => value.ToString("");

       
        }

        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels2 { get; set; }
        public Func<double, string> YFormatter2 { get; set; }

        #endregion
        //end region for cartesian chart for SPEED

        private void piechartinit()
        {
            piechart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Vehicle 1",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                    
                },
                new PieSeries
                {
                    Title = "Vehicle 2",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                },
                new PieSeries
                {
                    Title = "Vehicle 3",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                },
                new PieSeries
                {
                    Title = "Vehicle 4",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                },

                new PieSeries
                {
                    Title = "Vehicle 5",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                },

                new PieSeries
                {
                    Title = "Vehicle 6",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true,
                    StrokeThickness = 0,
                    PushOut = 0
                }
            };

        }

        public SeriesCollection Piechartcollection { get; set; }


        #region //window tool bar region


        private void AppsInfoButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Systemapps Version 8.4.20");
           // MessageBox.Show("Systemapps Version Alpha 2.0"); //14/7/2020
            MessageBox.Show("Systemapps Version Alpha 1.9.20"); //1/9/2020

            Debug.Write(Directory.GetCurrentDirectory());
        }

        private void AppsMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AppsPowerButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("ROI Points.txt"))
            {
                File.Delete("ROI Points.txt");

            }

            else if (File.Exists("ROI Points.bmp"))
            {
                File.Delete("ROI Points.bmp");
            }

            else if (File.Exists("url.txt"))
            {
                File.Delete("url.txt");
            }



            Application.Current.Shutdown();
        }

        private void GridToolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #endregion 
        //end region for window tool bar


        private void stuffchangewhendbcontrue()
        {
            TestConnButton.IsEnabled = false;
            Dbipaddresstestconn.IsEnabled = false;
            Dbusernametestconn.IsEnabled = false;
            Dbnametestconn.IsEnabled = false;
            Dbpasswordtestconn.IsEnabled = false;
            refreshcart.IsEnabled = true;
            pause.IsEnabled = true;
            autoref.IsEnabled = true;
            configparameterenabled.Visibility = Visibility.Visible;

            dashboardsignalstatus.Foreground = new SolidColorBrush(Colors.Green);
            Connectionstatus.Text = "Data Connected";
            packicondbconnectionstatus.Kind = PackIconKind.DatabaseCheck;
            packicondbconnectionstatus.Foreground = new SolidColorBrush(Colors.Green);
            dasboardtab.Visibility = Visibility.Visible;

            cartchartdb.Visibility = Visibility.Visible;
            speedchartcart.Visibility = Visibility.Visible;

        }
        private void TestConnButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string dbiptmp = Dbipaddresstestconn.Text;
                string dbnametmp = Dbnametestconn.Text;
                string dbusernametmp = Dbusernametestconn.Text;
                string dbpasstmp = Dbpasswordtestconn.Password;

                this.Cursor = Cursors.Wait;
                bool dbconnection = Functions.TestDbConnection(dbiptmp, dbnametmp, dbusernametmp, dbpasstmp);
                this.Cursor = Cursors.Arrow;

                if (dbconnection)
                {
                    stuffchangewhendbcontrue();

                    getvalueforgraphing();
                    isConnectionTested = true;
                    ErrorDialog obj = new ErrorDialog();
                    obj.Owner = this;
                    obj.Title = "Info";
                    obj.errorDialogMessage.Text = "Connection OK!";
                    obj.ShowDialog();


                    updatealldatagrid();
                    cartchartinit();
                    cartinit2();

                    dothis();
                    cbbanalyticfiltersearch.SelectedIndex = 0;
                    DataContext = Labels;
                    DataContext = Labels2;
                    DataContext = this;


                }
                else
                {
                    isConnectionTested = false;
                    ErrorDialog obj = new ErrorDialog();
                    obj.Owner = this;
                    obj.Title = "Info";
                    obj.errorDialogMessage.Text = "Connection Fail!";
                    obj.ShowDialog();
                    packicondbconnectionstatus.Kind = PackIconKind.DatabaseRemove;
                    packicondbconnectionstatus.Foreground = new SolidColorBrush(Colors.Red);

                }

                if (isConnectionTested == true)
                {
                    tabcontrolparameter.IsEnabled = true;

                    dothis();

                    dt.Start();
                }
                else
                {
                    tabcontrolparameter.IsEnabled = false;
                    dt.Stop();


                }
            }
            catch (Exception ex)
            {

            }

        }

        private void SaveTestDbConnButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Dbipaddresstestconn.Text == "" || Dbnametestconn.Text == "" || Dbusernametestconn.Text == "" || Dbpasswordtestconn.Password == "" || !isConnectionTested)
                {
                    Ok_Cancel_Dialog obj1 = new Ok_Cancel_Dialog();
                    obj1.Owner = this;
                    obj1.Title = "Alert";
                    obj1.errorDialogMessage.Text = "Some fields is empty/Connectiom not tested, are you sure you want to save?";
                    obj1.ShowDialog();

                    if (obj1.dialogOperation == (int)Operation.OK)
                    {
                        Settings.Default.dbipaddresstestconn = Dbipaddresstestconn.Text;
                        Settings.Default.dbnametestconn = Dbnametestconn.Text;
                        Settings.Default.dbusernametestconn = Dbusernametestconn.Text;
                        Settings.Default.dbpasswordtestconn = Dbpasswordtestconn.Password;
                        Settings.Default.Save();

                        ErrorDialog obj = new ErrorDialog();
                        obj.Owner = this;
                        obj.Title = "Info";
                        obj.errorDialogMessage.Text = "Connection Saved";
                        obj.ShowDialog();
                    }


                }
                else
                {
                    Settings.Default.dbipaddresstestconn = Dbipaddresstestconn.Text;
                    Settings.Default.dbnametestconn = Dbnametestconn.Text;
                    Settings.Default.dbusernametestconn = Dbusernametestconn.Text;
                    Settings.Default.dbpasswordtestconn = Dbpasswordtestconn.Password;
                    Settings.Default.Save();

                    ErrorDialog obj = new ErrorDialog();
                    obj.Owner = this;
                    obj.Title = "Info";
                    obj.errorDialogMessage.Text = "Connection Saved";
                    obj.ShowDialog();
                }
            }

            catch (Exception ex)
            {

            }
        }

        private void ResetTestConnButton_Click(object sender, RoutedEventArgs e)
        {
            Dbipaddresstestconn.Text = "";
            Dbnametestconn.Text = "";
            Dbusernametestconn.Text = "";
            Dbpasswordtestconn.Password = "";
        }

        private void DBConnect_TextChanged(object sender, RoutedEventArgs e)
        {
            tabcontrolparameter.IsEnabled = false;
            packicondbconnectionstatus.Kind = PackIconKind.DatabaseRemove;
            packicondbconnectionstatus.Foreground = new SolidColorBrush(Colors.Red);
            isConnectionTested = false;
            resetallfieldparameter();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Form1 form = new Form1();
            WindowInteropHelper wih = new WindowInteropHelper(this);
            wih.Owner = form.Handle;


            if (comboboxsourceoption.SelectedItem == comboboxvideo)
            {
                takesnapshotvideo();
            }


            bool visible = false;
            Debug.WriteLine(visible);

            visible = form.Visible;

            Debug.WriteLine(visible);

            form.Show();


        }

        private void comboboximage_Selected(object sender, RoutedEventArgs e)
        {
            notecam1.Visibility = Visibility.Visible;
            notecam2.Visibility = Visibility.Visible;
            imgfoldertxtbox.IsReadOnly = false;
            browseimagefolderbutton.IsEnabled = true;
            browsevideofolderbutton.IsEnabled = false;
            videofoldertxtbox.IsReadOnly = true;
            //filesextensiontxtbox.IsEnabled = true;
            //cameramasktxtbox.IsEnabled = true;
            cameraurltxtbox.IsReadOnly = true;
            drawroibutton.IsEnabled = false; ;
            //refreshmaskbutton.IsEnabled = true;
            //  nextimgeprevbuttton.Visibility = Visibility.Visible;
            //  previousimgbutton.Visibility = Visibility.Visible;
            imgfoldertxtbox.Text = "";
            videofoldertxtbox.Text = "";
            filesextensiontxtbox.Text = "";
            cameraurltxtbox.Text = "";
            cameramasktxtbox.Text = "";
            imageprevbox.Visibility = Visibility.Visible;
            videocameratab.Visibility = Visibility.Collapsed;
            //  playvideobutton.Visibility = Visibility.Collapsed;
            //  pausevideobutton.Visibility = Visibility.Collapsed;
            videocameratab.Stop();
            // StopVideobutton.Visibility = Visibility.Collapsed;
            //  Takesnapshotvideo.Visibility = Visibility.Collapsed;
            // prevvideobutton.Visibility = Visibility.Collapsed;
            // nextvideobutton.Visibility = Visibility.Collapsed;
            checkcam.IsEnabled = false;

        }

        private void browseimagefolderbutton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"imagepath.txt", "");
            drawroibutton.IsEnabled = true;
            counterimg = 0;
            imgfilepath.Clear();
            Winforms.FolderBrowserDialog FolderDialog = new Winforms.FolderBrowserDialog();
            FolderDialog.ShowNewFolderButton = false;
            FolderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            Winforms.DialogResult result = FolderDialog.ShowDialog();

            try
            {
                if (result == Winforms.DialogResult.OK)
                {
                    string fpath = FolderDialog.SelectedPath;
                    imagefolderdir = fpath;
                    string editedfpath = fpath.Replace("\\", "\\\\");
                    imgfoldertxtbox.Text = editedfpath;
                    DirectoryInfo folder = new DirectoryInfo(fpath);


                    if (folder.Exists)
                    {
                        imagefolderdir = fpath;
                        imageprevbox.Visibility = Visibility.Visible;

                        foreach (FileInfo fileinfo in folder.GetFiles())
                        {

                            imgfilepath.Add(fileinfo.Name);
                            string sDate = fileinfo.CreationTime.ToString("yyyy-MM-dd");
                            Debug.WriteLine("@Debug File " + fileinfo.Name + " Date " + sDate);
                            Debug.WriteLine(imgfilepath);
                        }

                        imageprevbox.Source = new BitmapImage(new Uri(fpath + "/" + imgfilepath[counterimg]));
                        DirectoryPathbelowprevbox.Text = imgfilepath[0];

                        File.WriteAllText(@"imagepath.txt", fpath + "/" + imgfilepath[counterimg]);
                        string test = imgfilepath[counterimg];
                        // Debug.WriteLine(test.Substring(0, test.LastIndexOf(".") + 1));
                        Debug.WriteLine(test.Split('.').Last());
                        filesextensiontxtbox.Text = "." + test.Split('.').Last();



                    }


                }
            }

            catch (Exception ex)
            {
                imgfoldertxtbox.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void comboboxvideo_Selected(object sender, RoutedEventArgs e)
        {
            notecam1.Visibility = Visibility.Visible;
            notecam2.Visibility = Visibility.Visible;
            drawroibutton.IsEnabled = false;
            imgfoldertxtbox.IsReadOnly = true;
            browseimagefolderbutton.IsEnabled = false;
            browsevideofolderbutton.IsEnabled = true;
            videofoldertxtbox.IsReadOnly = false;
            //filesextensiontxtbox.IsEnabled = true;
            cameramasktxtbox.IsReadOnly = true;
            cameraurltxtbox.IsReadOnly = true;
            //drawroibutton.IsEnabled = true;
            //refreshmaskbutton.IsEnabled = true;
            // nextimgeprevbuttton.Visibility = Visibility.Collapsed;
            // previousimgbutton.Visibility = Visibility.Collapsed;
            imgfoldertxtbox.Text = "";
            videofoldertxtbox.Text = "";
            filesextensiontxtbox.Text = "";
            cameraurltxtbox.Text = "";
            imageprevbox.Visibility = Visibility.Collapsed;
            videocameratab.Visibility = Visibility.Visible;
            cameramasktxtbox.Text = "";
            //  playvideobutton.Visibility = Visibility.Visible;
            // pausevideobutton.Visibility = Visibility.Visible;
            // StopVideobutton.Visibility = Visibility.Visible;
            // Takesnapshotvideo.Visibility = Visibility.Visible;
            // prevvideobutton.Visibility = Visibility.Visible;
            // nextvideobutton.Visibility = Visibility.Visible;
            checkcam.IsEnabled = false;

        }

        private void comboboxcamera_Selected(object sender, RoutedEventArgs e)
        {
            notecam1.Visibility = Visibility.Collapsed;
            notecam2.Visibility = Visibility.Collapsed;
            imgfoldertxtbox.IsReadOnly = true;
            browseimagefolderbutton.IsEnabled = false;
            browsevideofolderbutton.IsEnabled = false;
            videofoldertxtbox.IsReadOnly = true;
            filesextensiontxtbox.IsReadOnly = true;
            cameramasktxtbox.IsReadOnly = true;
            cameraurltxtbox.IsReadOnly = false;
            drawroibutton.IsEnabled = false; ;
            //refreshmaskbutton.IsEnabled = true;
            // nextimgeprevbuttton.Visibility = Visibility.Collapsed;
            //previousimgbutton.Visibility = Visibility.Collapsed;
            imgfoldertxtbox.Text = "";
            videofoldertxtbox.Text = "";
            filesextensiontxtbox.Text = "";
            cameraurltxtbox.Text = "";
            cameramasktxtbox.Text = "";
            videocameratab.Visibility = Visibility.Collapsed;
            imageprevbox.Visibility = Visibility.Collapsed;
            //playvideobutton.Visibility = Visibility.Collapsed;
            // pausevideobutton.Visibility = Visibility.Collapsed;
            videocameratab.Stop();
            // StopVideobutton.Visibility = Visibility.Collapsed;
            // Takesnapshotvideo.Visibility = Visibility.Collapsed;
            // prevvideobutton.Visibility = Visibility.Collapsed;
            // nextvideobutton.Visibility = Visibility.Collapsed;
            checkcam.IsEnabled = true;
        }

        private void refremask()
        {

            if (File.Exists("ROI Points.txt"))
            {
                // cameramasktxtbox.Text = File.ReadLines("ROI Points.txt").Take(2).First();
                cameramasktxtbox.Text = File.ReadAllText("ROI Points.txt");
            }

            /* else
             {
                 ErrorDialog obj = new ErrorDialog();
                 obj.Owner = this;
                 obj.Title = "Info";
                 obj.errorDialogMessage.Text = "Please Draw ROI first!";
                 obj.ShowDialog();

             }*/
        }

        private void browsevideofolderbutton_Click(object sender, RoutedEventArgs e)
        {
            drawroibutton.IsEnabled = true;
            countervid = 0;
            vidfilepath.Clear();
            Winforms.FolderBrowserDialog FolderDialog = new Winforms.FolderBrowserDialog();
            FolderDialog.ShowNewFolderButton = false;
            FolderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            Winforms.DialogResult result = FolderDialog.ShowDialog();

            try
            {
                if (result == Winforms.DialogResult.OK)
                {
                    string fpath = FolderDialog.SelectedPath;
                    vidfolderdir = fpath;
                    string editedfpath = fpath.Replace("\\", "\\\\");
                    videofoldertxtbox.Text = editedfpath;
                    DirectoryInfo folder = new DirectoryInfo(fpath);

                    if (folder.Exists)
                    {
                        videocameratab.Visibility = Visibility.Visible;
                        foreach (FileInfo fileinfo in folder.GetFiles())
                        {
                            vidfilepath.Add(fileinfo.Name);
                            string sDate = fileinfo.CreationTime.ToString("yyyy-MM-dd");
                            Debug.WriteLine("@Debug File " + fileinfo.Name + " Date " + sDate);
                            Debug.WriteLine(vidfilepath);
                        }
                        videocameratab.Source = new Uri(fpath + "/" + vidfilepath[countervid]);
                        DirectoryPathbelowprevbox.Text = vidfilepath[countervid];
                        string test = vidfilepath[countervid];
                        filesextensiontxtbox.Text = "." + test.Split('.').Last();
                        videocameratab.Play();
                        videocameratab.Position = new TimeSpan(0, 0, 0, 50);
                        int milliseconds = 2000;
                        Thread.Sleep(milliseconds);

                        videocameratab.Pause();

                    }




                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void nextimgeprevbuttton_Click(object sender, RoutedEventArgs e)
        {
            int totalimginfolder = imgfilepath.Count();
            counterimg = counterimg + 1;

            try
            {
                if (counterimg == totalimginfolder)
                {
                    counterimg = 0;
                }
                imageprevbox.Source = new BitmapImage(new Uri(imagefolderdir + "/" + imgfilepath[counterimg]));
                DirectoryPathbelowprevbox.Text = imgfilepath[counterimg];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Folder with images first");
                Debug.WriteLine(ex.Message);
            }



        }

        private void previousimgbutton_Click(object sender, RoutedEventArgs e)
        {
            int totalimginfolder = imgfilepath.Count();
            counterimg = counterimg - 1;

            try
            {
                if (counterimg < 0)
                {
                    counterimg = totalimginfolder - 1;
                }
                imageprevbox.Source = new BitmapImage(new Uri(imagefolderdir + "/" + imgfilepath[counterimg]));
                DirectoryPathbelowprevbox.Text = imgfilepath[counterimg];

            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select folder with images first");
                Debug.WriteLine(ex.Message);
            }


        }

        private void ResetAllField_Click(object sender, RoutedEventArgs e)
        {
            imgfoldertxtbox.Text = "";
            videofoldertxtbox.Text = "";
            filesextensiontxtbox.Text = "";
            cameraurltxtbox.Text = "";
            cameramasktxtbox.Text = "";
            imageprevbox.Visibility = Visibility.Collapsed;
            videocameratab.Visibility = Visibility.Collapsed;
            videocameratab.Stop();
            DirectoryPathbelowprevbox.Text = "";
        }

        private void pausevideobutton_Click(object sender, RoutedEventArgs e)
        {
            videocameratab.Pause();

        }

        private void playvideobutton_Click(object sender, RoutedEventArgs e)
        {
            videocameratab.Play();
            videocameratab.Position = new TimeSpan(0, 0, 0, 50);

        }

        private void Takesnapshotvideo_Click(object sender, RoutedEventArgs e)
        {

            //videoscreenshot vidss = new videoscreenshot();

            // vidss.Show();
            int vidheight = videocameratab.NaturalVideoHeight;
            int vidwidth = videocameratab.NaturalVideoWidth;

            string root = "snapshot";

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            Directory.SetCurrentDirectory(root);

            Size dpi = new Size(96, 96);
            RenderTargetBitmap bmp =
              new RenderTargetBitmap(480, 360,
                dpi.Width, dpi.Height, PixelFormats.Pbgra32);
            bmp.Render(videocameratab);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string filename = "Screenshot" + Guid.NewGuid().ToString().Substring(0, 5) + ".jpg";
            FileStream fs = new FileStream(filename, FileMode.Create);
            encoder.Save(fs);
            fs.Close();

            // Process.Start(filename);

        }

        private void StopVideobutton_Click(object sender, RoutedEventArgs e)
        {
            videocameratab.Stop();

        }

        private void nextvideobutton_Click(object sender, RoutedEventArgs e)
        {
            int totalvidinfolder = vidfilepath.Count();
            countervid = countervid + 1;

            try
            {
                if (countervid == totalvidinfolder)
                {
                    countervid = 0;
                }
                videocameratab.Source = new Uri(vidfolderdir + "/" + vidfilepath[countervid]);
                DirectoryPathbelowprevbox.Text = vidfilepath[countervid];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a folder with video first");
                Debug.WriteLine(ex.Message);

            }


        }

        private void prevvideobutton_Click(object sender, RoutedEventArgs e)
        {
            int totalvidinfolder = vidfilepath.Count();
            countervid = countervid - 1;
            try
            {

                if (countervid < 0)
                {
                    countervid = totalvidinfolder - 1;
                }
                videocameratab.Source = new Uri(vidfolderdir + "/" + vidfilepath[countervid]);
                DirectoryPathbelowprevbox.Text = vidfilepath[countervid];
            }

            catch (Exception ex)
            {
                MessageBox.Show("Please select a folder with video first");
                Debug.WriteLine(ex.Message);

            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DirectoryPathbelowprevbox.Text = "";
            imageprevbox.Visibility = Visibility.Collapsed;
            videocameratab.Visibility = Visibility.Collapsed;


        }

        private void createnewdbbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                string newSourceServerIp = newdbip.Text;
                string newSourceDbName = newdbname.Text;
                string newSourceDbUser = newdbusername.Text;
                string newSourceDbPassword = newdbpassword.Password;

                if (newSourceServerIp.Equals("") || newSourceDbName.Equals("") || newSourceDbUser.Equals("") || newSourceDbPassword.Equals(""))
                {
                    ErrorDialog obj = new ErrorDialog();
                    obj.Owner = this;
                    obj.Title = "Error";
                    obj.errorDialogMessage.Text = "All Fields Must Be Filled";
                    obj.ShowDialog();
                }
                else
                {
                    if (!IsValidIPv4(newSourceServerIp))
                    {
                        ErrorDialog obj = new ErrorDialog();
                        obj.Owner = this;
                        obj.Title = "Error";
                        obj.errorDialogMessage.Text = "Enter Valid Server IP";
                        obj.ShowDialog();
                    }
                    else
                    {
                        this.Cursor = Cursors.Wait;
                        bool bDBConnection = Functions.CreateNewDatabase(newSourceServerIp, newSourceDbName, newSourceDbUser, newSourceDbPassword);
                        this.Cursor = Cursors.Arrow;
                        if (bDBConnection)
                        {
                            Mouse.OverrideCursor = null;
                            ErrorDialog obj = new ErrorDialog();
                            obj.Owner = this;
                            obj.Title = "Info";
                            obj.errorDialogMessage.Text = "Database Created Successfully";

                            obj.ShowDialog();
                        }
                        else
                        {
                            Mouse.OverrideCursor = null;
                            ErrorDialog obj = new ErrorDialog();
                            obj.Owner = this;
                            obj.Title = "Error";
                            obj.errorDialogMessage.Text = "There is error in creating database. Current DB will be dropped";
                            obj.ShowDialog();
                            Functions.DropDatabase(newSourceServerIp, newSourceDbName, newSourceDbUser, newSourceDbPassword);

                        }

                    }
                }


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                // Logic.Helper.AddtoLogFile(ex.Message, "NewCreateButton_Click::Exception");
                ErrorDialog obj = new ErrorDialog();
                obj.Owner = this;
                obj.Title = "Error";
                obj.errorDialogMessage.Text = "Error Creating Database" + ex.Message;
                Mouse.OverrideCursor = null;
                obj.ShowDialog();
                //DropDatabase();
            }


        }

        private void resetnewdbfields_Click(object sender, RoutedEventArgs e)
        {
            newdbip.Text = "";
            newdbname.Text = "";
            newdbusername.Text = "";
            newdbpassword.Password = "";
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

        private void dropdbdeletebutton_Click(object sender, RoutedEventArgs e)
        {

            string dropSourceServerIp = dropdbip.Text;
            string dropSourceDbName = dropdbname.Text;
            string dropSourceDbUser = dropdbusername.Text;
            string dropSourceDbPassword = dropdbpassword.Password;

            bool bDBConnection = false;
            try
            {

                if (dropSourceServerIp.Equals("") || dropSourceDbName.Equals("") || dropSourceDbUser.Equals("") || dropSourceDbPassword.Equals(""))
                {
                    ErrorDialog obj = new ErrorDialog();
                    obj.Owner = this;
                    obj.Title = "Error";
                    obj.errorDialogMessage.Text = "All Fields Must Be Filled";
                    obj.ShowDialog();
                }

                else
                {
                    if (!IsValidIPv4(dropSourceServerIp))
                    {
                        ErrorDialog obj = new ErrorDialog();
                        obj.Owner = this;
                        obj.Title = "Error";
                        obj.errorDialogMessage.Text = "Enter Valid Server IP";
                        obj.ShowDialog();
                    }

                    else
                    {
                        Ok_Cancel_Dialog obj1 = new Ok_Cancel_Dialog();
                        obj1.Owner = this;
                        obj1.Title = "Info";
                        obj1.errorDialogMessage.Text = "Confirm drop " + dropSourceDbName + "?";
                        obj1.ShowDialog();

                        if (obj1.dialogOperation == (int)Operation.OK)
                        {
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            this.Cursor = Cursors.Wait;
                            bDBConnection = Functions.DropDatabase(dropSourceServerIp, dropSourceDbName, dropSourceDbUser, dropSourceDbPassword);
                            this.Cursor = Cursors.Arrow;

                            if (bDBConnection == true)
                            {
                                Mouse.OverrideCursor = null;
                                ErrorDialog obj = new ErrorDialog();
                                obj.Owner = this;
                                obj.Title = "Error";
                                obj.errorDialogMessage.Text = "Database Dropped Successfully";
                                obj.ShowDialog();

                            }

                        }



                        else
                        {
                            Mouse.OverrideCursor = null;
                            ErrorDialog obj = new ErrorDialog();
                            obj.Owner = this;
                            obj.Title = "Error";
                            obj.errorDialogMessage.Text = "There is error in dropping database.";
                            obj.ShowDialog();

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                this.Cursor = Cursors.Arrow;
                ErrorDialog obj = new ErrorDialog();
                obj.Owner = this;
                obj.Title = "Error";
                obj.errorDialogMessage.Text = "Error Dropping Database" + ex.Message;
                obj.ShowDialog();
            }

        }

        private void resetdropdbbutton_Click(object sender, RoutedEventArgs e)
        {
            dropdbip.Text = "";
            dropdbname.Text = "";
            dropdbusername.Text = "";
            dropdbpassword.Password = "";

        }

        private void checkcam_Click(object sender, RoutedEventArgs e)
        {


            if (cameraurltxtbox.Text == "")
            {
                MessageBox.Show("Camera URI field is empty");
            }

            else
            {
                File.WriteAllText("camurl.txt", cameraurltxtbox.Text);
                try
                {
                    drawroibutton.IsEnabled = true;
                    vlcwpf vlcwpf = new vlcwpf();
                    vlcwpf.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please make sure this machine have VLC Media Player.");
                    throw ex;
                }
            }
        }

        private void takesnapshotvideo()
        {
            //videoscreenshot vidss = new videoscreenshot();

            // vidss.Show();
            int vidheight = videocameratab.NaturalVideoHeight;
            int vidwidth = videocameratab.NaturalVideoWidth;

            string root = "snapshot";

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            Directory.SetCurrentDirectory(root);

            Size dpi = new Size(96, 96);
            RenderTargetBitmap bmp =
              new RenderTargetBitmap(480, 360,
                dpi.Width, dpi.Height, PixelFormats.Pbgra32);
            bmp.Render(videocameratab);

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string filename = "Screenshot" + Guid.NewGuid().ToString().Substring(0, 5) + ".jpg";

            File.WriteAllText(@"imagepath.txt", filename);

            Debug.WriteLine(Directory.GetCurrentDirectory());



            FileStream fs = new FileStream(filename, FileMode.Create);
            encoder.Save(fs);
            fs.Close();

            //Process.Start(filename);
        }

        public static string GetLocalIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();

            }
            return localIP;
        }

        private void datagridaddrowbutton_Click(object sender, RoutedEventArgs e)
        {
            switch (tabcontrolparameter.SelectedIndex)
            {
                case 0: //done
                    {
                        MessageBoxResult result = MessageBox.Show("Confirm add new Row with current local IP?", "Add Row", MessageBoxButton.YesNoCancel);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                {
                                    //string localip = GetLocalIPAddress();
                                    // string sqlstatement = "INSERT INTO server_info (ip_address) VALUES('"+localip+"')";
                                    //this.AUDserverinfo(sqlstatement, 0);

                                    string ipaddress = GetLocalIPAddress();
                                    string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

                                    string sql = "SELECT COUNT(*) FROM server_info WHERE ip_address = @ipaddress";
                                    using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                                    {
                                        cn.Open();
                                        using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                                        {
                                            cmd.Parameters.AddWithValue("@ipaddress", ipaddress);
                                            var aresult = Convert.ToInt32(cmd.ExecuteScalar());

                                            string checkid = "SELECT id FROM server_info WHERE ip_address = @ipaddress";
                                            MySqlCommand cmd2 = new MySqlCommand(checkid, cn);
                                            cmd2.Parameters.AddWithValue("ipaddress", ipaddress);
                                            string aresult2 = Convert.ToString(cmd2.ExecuteScalar());
                                            if (aresult > 0)
                                            {
                                                MessageBox.Show("Record already exist at id = " + aresult2);

                                            }
                                            else
                                            {
                                                string sqlstatement = "INSERT INTO server_info (ip_address) VALUES(\"" + ipaddress + "\");";
                                                this.AUDserverinfo(sqlstatement, 0);
                                                MessageBox.Show("Successfully added ip address: " + ipaddress);
                                            }

                                        }
                                        cn.Close();
                                    }
                                }

                                break;

                            case MessageBoxResult.No:
                                {

                                    custonipaddress newwindow = new custonipaddress();
                                    newwindow.ShowDialog();
                                    newwindow.Topmost = true;

                                    string customnewip = newwindow.returnvalue();

                                    string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

                                    string sql = "SELECT COUNT(*) FROM server_info WHERE ip_address = @ipaddress";
                                    using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                                    {
                                        cn.Open();
                                        using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                                        {
                                            cmd.Parameters.AddWithValue("@ipaddress", customnewip);
                                            var aresult = Convert.ToInt32(cmd.ExecuteScalar());

                                            string checkid = "SELECT id FROM server_info WHERE ip_address = @ipaddress";
                                            MySqlCommand cmd2 = new MySqlCommand(checkid, cn);
                                            cmd2.Parameters.AddWithValue("@ipaddress", customnewip);
                                            string aresult2 = Convert.ToString(cmd2.ExecuteScalar());
                                            if (aresult > 0)
                                            {
                                                MessageBox.Show("Record already exist at id = " + aresult2);

                                            }
                                            else
                                            {
                                                string sqlstatement = "INSERT INTO server_info (ip_address) VALUES('" + customnewip + "');";
                                                this.AUDserverinfo(sqlstatement, 0);
                                                MessageBox.Show("Successfully added ip address: " + customnewip + "");

                                            }

                                        }
                                        cn.Close();
                                    }


                                }

                                break;
                            case MessageBoxResult.Cancel:

                                break;
                        }

                    }
                    break;

                case 1: //done
                    {
                        MessageBoxResult result = MessageBox.Show("Confirm add new Row with current paramaters?", "Add Row", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                {

                                    string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                                    using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                                    {
                                        cn.Open();
                                        if (comboboxsourceoption.SelectedIndex == 0 || comboboxsourceoption.SelectedIndex == 1)
                                        {

                                            string sqlstatement = "INSERT INTO camera (cam_source,cam_url,cam_mask,cam_ext) VALUES(@cam_source,@cam_url,@cam_mask,@cam_ext);";
                                            this.AUDcamera(sqlstatement, 0);

                                        }

                                        else
                                        {
                                            string sqlstatement = "INSERT INTO camera (cam_source,cam_url,cam_mask,cam_ext) VALUES(@cam_source,@cam_url,@cam_mask,\"\");";
                                            this.AUDcamera(sqlstatement, 0);


                                        }

                                        cn.Close();

                                    }

                                   try
                                    {
                                        // Check if file exists with its full path    
                                        if (File.Exists("Roi Points.txt"))
                                        {
                                            // If file found, delete it    
                                            File.Delete("Roi Points.txt");

                                        }
                                        else Debug.WriteLine("Roi Points.txt not found");
                                        
                                    }
                                    catch (IOException ioExp)
                                    {
                                        MessageBox.Show(ioExp.Message);
                                    }

                                }
                                break;

                            case MessageBoxResult.No:
                                {
                                    break;

                                }


                        }


                    }

                    break;

                case 2: //done
                    {
                        string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                        using (MySqlConnection cn = new MySqlConnection(myConnectionString))

                        {
                            cn.Open();

                            if (inputsourcesourcetextbox.Text.Length > 10)
                            {

                                MessageBox.Show("Exceeded Source string length");
                                resetallfieldparameter();
                            }

                            else
                            {

                                string sqlstatement = "INSERT INTO input_source (source) VALUES(@source);";
                                this.AUDinputsource(sqlstatement, 0);
                            }


                            cn.Close();

                        }
                    }
                    break;

                case 3: //done
                    {
                        MessageBoxResult result = MessageBox.Show("Confirm add new Row with current paramaters?", "Add Row", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                {
                                    if (cbbdetmodelid.Text == "" || cbbpredmodelid.Text == "" || cbbserverid.Text == "" || cbbcameraid.Text == "")
                                    {
                                        MessageBox.Show("Please ensure the Det. Model ID/Pred. Model ID/Server ID/Camera ID are all selected");
                                    }

                                    else
                                    {
                                        string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                                        using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                                        {

                                            cn.Open();

                                            //string sqlstatement = "INSERT INTO analytic (det_conf, det_debug, trk_debug, pred_enable, status, det_device_id, pred_device_id, det_model_id, pred_model_id, server_id, camera_id) VALUES(@det_conf, @det_debug, @trk_debug, @pred_enable, @status,@det_device_id, @pred_device_id, @det_model_id, @pred_model_id, @server_id, @camera_id);";

                                            string sqlstatement = "INSERT INTO analytic (det_conf, det_debug, trk_debug, pred_enable, status, det_device_id, pred_device_id, skip_outer, skip_inner, init,last,initVideo,lastVideo, processDuration, det_model_id, pred_model_id, server_id, camera_id) VALUES(@det_conf, @det_debug, @trk_debug, @pred_enable, @status,@det_device_id, @pred_device_id, @skip_outer, @skip_inner,@init,@last,@initVideo,@lastVideo, @processDuration, @det_model_id, @pred_model_id, @server_id, @camera_id);";

                                            this.AUDanalytics(sqlstatement, 0);


                                            cn.Close();

                                        }
                                    }



                                }
                                break;

                            case MessageBoxResult.No:
                                {
                                    break;

                                }


                        }

                    }
                    break;

                case 4: //done
                    {
                        MessageBoxResult result = MessageBox.Show("Confirm add new Row with current paramaters?", "Add Row", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                {

                                    string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                                    using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                                    {
                                        try
                                        {

                                            if (comboboxanalyticidstat.Text == "" )
                                            {
                                                MessageBox.Show("Please choose the analytic id and camera id first");
                                            }
                                            else
                                            {
                                                cn.Open();
                                                string date = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                                                string time = DateTime.Now.ToString("HH:mm:ss ");
                                                string sqlstatement = "INSERT INTO statistics (analytic_id,date,time,total_vehicle,vehi_1,vehi_2,vehi_3,vehi_4, vehi_5, vehi_6, avg_vehicle_speed, avg_vehicle_gap) VALUES (@analytic_id,@date,@time, @total_vehicle,@vehi_1,@vehi_2,@vehi_3,@vehi_4, @vehi_5, @vehi_6,@avg_vehicle_speed, @avg_vehicle_gap);";
                                                this.AUDstatistics(sqlstatement, 0);


                                                cn.Close();
                                                dothis();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }



                                }
                                break;

                            case MessageBoxResult.No:
                                {
                                    break;

                                }


                        }

                    }
                    break;

                case 5: //done
                    {
                        string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                        using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                        {
                            cn.Open();

                            if (predframeworktxtbox.Text == "" || predsizetxtbox.Text == "" || predsourcetxtbox.Text == "" || predmodeltxbox.Text == "" || predlabeltxtbox.Text == "" || predlabelmappertxtbox.Text == "")
                            {
                                MessageBox.Show("Field in all parameter first!");
                            }
                            else
                            {

                                string sqlstatement = "INSERT INTO prediction (pred_framework,pred_source,pred_size,pred_model,pred_label,pred_labelmap) VALUES(@pred_framework,@pred_source,@pred_size,@pred_model,@pred_label,@pred_labelmap);";
                                this.AUDpredictionmodule(sqlstatement, 0);

                            }


                            cn.Close();

                        }

                    }
                    break;

                case 6: //done
                    {
                        string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                        using (MySqlConnection cn = new MySqlConnection(myConnectionString))
                        {
                            cn.Open();

                            if (detframeworktxtbox.Text == "" || detsizetxtbox.Text == "" || detsourcetxtbox.Text == "" || detmodeltxtbox.Text == "" || detlabeltxtbox.Text == "" || detlabelmappertxtbox.Text == "")
                            {
                                MessageBox.Show("Field in all parameter first!");
                            }
                            else
                            {

                                string sqlstatement = "INSERT INTO detection (det_framework,det_source,det_size,det_model,det_label,det_labelmap) VALUES(@det_framework,@det_source,@det_size,@det_model,@det_label,@det_labelmap);";
                                this.AUDdetectionmodule(sqlstatement, 0);

                            }


                            cn.Close();

                        }

                    }
                    break;
            }


            //adddata();
            //adddatadialog adddata = new adddatadialog();
            //adddata.Owner = this;  
        }

        public void FillGrid()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            string query = "SELECT id,ip_address FROM server_info";
            MySqlConnection con = new MySqlConnection(myConnectionString);

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "server_info");
                serverinfodatagrid.DataContext = ds;
                /* MySqlCommand cmd = con.CreateCommand();
                 cmd.CommandText = query;
                 cmd.CommandType = CommandType.Text;
                 MySqlDataReader dr = cmd.ExecuteReader();
                 DataTable dt = new DataTable();
                 dt.Load(dr);
                 serverinfodatagrid.ItemsSource = dt.DefaultView;
                 dr.Close(); */

                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void FillGrid(string query, DataGrid grid, string dbname)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            //string query = "SELECT id,ip_address FROM server_info";
            MySqlConnection con = new MySqlConnection(myConnectionString);

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, dbname);
                grid.DataContext = ds;


                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }




        }

        #region //AUD region

        public void AUDserverinfo(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        msg = "New Row Inserted";
                        // cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(serverinfoidtextbox.Text);
                        cmd.Parameters.Add("ip_address", MySqlDbType.VarChar).Value = serverinfoiptextbox.Text;

                    }
                    break;
                case 1:
                    {
                        msg = "Row Updated";
                        cmd.Parameters.Add("ip_address", MySqlDbType.VarChar).Value = serverinfoiptextbox.Text;

                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(serverinfoidtextbox.Text);

                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AUDcamera(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        msg = "New Row Inserted";
                        if (comboboxsourceoption.SelectedIndex == 0)
                        {
                            cmd.Parameters.Add("cam_source", MySqlDbType.Int32).Value = 1;
                            string newfolderpath;
                            newfolderpath = imgfoldertxtbox.Text.Replace("\\\\", "\\");
                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = newfolderpath;
                            cmd.Parameters.Add("cam_ext", MySqlDbType.VarChar).Value = filesextensiontxtbox.Text;
                            cmd.Parameters.Add("cam_mask", MySqlDbType.VarChar).Value = cameramasktxtbox.Text;
                        }
                        else if (comboboxsourceoption.SelectedIndex == 1)
                        {
                            string newfolderpath;
                            newfolderpath = videofoldertxtbox.Text.Replace("\\\\", "\\");
                            cmd.Parameters.Add("cam_source", MySqlDbType.Int32).Value = 2;
                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = newfolderpath;
                            cmd.Parameters.Add("cam_ext", MySqlDbType.VarChar).Value = filesextensiontxtbox.Text;
                            cmd.Parameters.Add("cam_mask", MySqlDbType.VarChar).Value = cameramasktxtbox.Text;
                        }
                        else if (comboboxsourceoption.SelectedIndex == 2)
                        {
                            cmd.Parameters.Add("cam_source", MySqlDbType.Int32).Value = 3;
                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = cameraurltxtbox.Text;
                            cmd.Parameters.Add("cam_mask", MySqlDbType.VarChar).Value = cameramasktxtbox.Text;

                        }
                        else
                        {
                            cmd.Parameters.Add("cam_source", MySqlDbType.Int32).Value = 1;
                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = "";
                            cmd.Parameters.Add("cam_ext", MySqlDbType.VarChar).Value = "";
                            cmd.Parameters.Add("cam_mask", MySqlDbType.VarChar).Value = "";
                        }


                    }
                    break;
                case 1:
                    {
                        msg = "Row Updated";
                        if (comboboxsourceoption.SelectedIndex == 0)
                        {
                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = imgfoldertxtbox.Text;

                            //cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = "\\\""+imgfoldertxtbox.Text+ "\"" ;
                            cmd.Parameters.Add("cam_ext", MySqlDbType.VarChar).Value = filesextensiontxtbox.Text;
                        }
                        else if (comboboxsourceoption.SelectedIndex == 1)
                        {

                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = videofoldertxtbox.Text;
                            cmd.Parameters.Add("cam_ext", MySqlDbType.VarChar).Value = filesextensiontxtbox.Text;
                        }
                        else if (comboboxsourceoption.SelectedIndex == 2)
                        {

                            cmd.Parameters.Add("cam_url", MySqlDbType.VarChar).Value = cameraurltxtbox.Text;

                        }
                        cmd.Parameters.Add("cam_mask", MySqlDbType.VarChar).Value = cameramasktxtbox.Text;

                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("cam_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cameraidtxtbox.Text);

                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }
        public void AUDinputsource(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        try
                        {
                            msg = "New Row Inserted";
                            //cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(inputsourceidtextbox.Text);
                            cmd.Parameters.Add("source", MySqlDbType.VarChar).Value = inputsourcesourcetextbox.Text;
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    break;
                case 1:
                    {
                        msg = "Row Updated";
                        cmd.Parameters.Add("source", MySqlDbType.VarChar).Value = inputsourcesourcetextbox.Text;

                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(inputsourceidtextbox.Text);

                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }

        public void AUDanalytics(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {


                        msg = "New Row Inserted";


                        cmd.Parameters.Add("det_conf", MySqlDbType.Decimal).Value = Convert.ToDecimal(det_conftxtblock.Text);


                        if (det_debugtrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("det_debug", MySqlDbType.Int32, 6).Value = 1;
                        }
                        else
                        {
                            cmd.Parameters.Add("det_debug", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (trkdebugtrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("trk_debug", MySqlDbType.Int32, 6).Value = 1;

                        }
                        else
                        {
                            cmd.Parameters.Add("trk_debug", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (predenabletrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("pred_enable", MySqlDbType.Int32, 6).Value = 1;

                        }
                        else
                        {
                            cmd.Parameters.Add("pred_enable", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (statustrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("status", MySqlDbType.Int32, 6).Value = 1;


                        }
                        else
                        {
                            cmd.Parameters.Add("status", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (detdevidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("det_device_id", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("det_device_id", MySqlDbType.Int32, 6).Value = Int32.Parse(detdevidtxtbox.Text);
                        }

                        if (preddevidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("pred_device_id", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("pred_device_id", MySqlDbType.Int32, 6).Value = Int32.Parse(preddevidtxtbox.Text);
                        }

                        if (skipoutertxtbox.Text == "")
                        {
                            cmd.Parameters.Add("skip_outer", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("skip_outer", MySqlDbType.Int32, 6).Value = Int32.Parse(skipoutertxtbox.Text);

                        }

                        if (skipinnertxtbox.Text == "")
                        {
                            cmd.Parameters.Add("skip_inner", MySqlDbType.Int32, 6).Value = 0;

                        }

                        else
                        {
                            cmd.Parameters.Add("skip_inner", MySqlDbType.Int32, 6).Value = Int32.Parse(skipinnertxtbox.Text);

                        }

                        if (inittxtbox.Text == "")
                        {
                            cmd.Parameters.Add("init", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("init", MySqlDbType.Int32, 6).Value = Int32.Parse(inittxtbox.Text);

                        }

                        if (lasttxtbox.Text == "")
                        {
                            cmd.Parameters.Add("last", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("last", MySqlDbType.Int32, 6).Value = Int32.Parse(lasttxtbox.Text);

                        }

                        if (initvidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("initVideo", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("initVideo", MySqlDbType.Int32, 6).Value = Int32.Parse(initvidtxtbox.Text);

                        }

                        if (lastvidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("lastVideo", MySqlDbType.Int32, 6).Value = 0;

                        }

                        else
                        {
                            cmd.Parameters.Add("lastVideo", MySqlDbType.Int32, 6).Value = Int32.Parse(lastvidtxtbox.Text);

                        }

                        if (procdurtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("processDuration", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("processDuration", MySqlDbType.Int32, 6).Value = Int32.Parse(procdurtxtbox.Text);

                        }

                        cmd.Parameters.Add("det_model_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbdetmodelid.Text);
                        cmd.Parameters.Add("pred_model_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbpredmodelid.Text);
                        cmd.Parameters.Add("server_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbserverid.Text);
                        cmd.Parameters.Add("camera_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbcameraid.Text);

                    }
                    break;
                case 1:
                    {

                        msg = "Row Updated";

                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Convert.ToInt32(analyticsidtxtbox.Text);

                        cmd.Parameters.Add("det_conf", MySqlDbType.Decimal).Value = Convert.ToDecimal(det_conftxtblock.Text);


                        if (det_debugtrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("det_debug", MySqlDbType.Int32, 6).Value = 1;
                        }
                        else
                        {
                            cmd.Parameters.Add("det_debug", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (trkdebugtrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("trk_debug", MySqlDbType.Int32, 6).Value = 1;

                        }
                        else
                        {
                            cmd.Parameters.Add("trk_debug", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (predenabletrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("pred_enable", MySqlDbType.Int32, 6).Value = 1;

                        }
                        else
                        {
                            cmd.Parameters.Add("pred_enable", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (statustrue.IsChecked == true)
                        {
                            cmd.Parameters.Add("status", MySqlDbType.Int32, 6).Value = 1;


                        }
                        else
                        {
                            cmd.Parameters.Add("status", MySqlDbType.Int32, 6).Value = 0;
                        }
                        //
                        if (detdevidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("det_device_id", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("det_device_id", MySqlDbType.Int32, 6).Value = Int32.Parse(detdevidtxtbox.Text);
                        }

                        if (preddevidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("pred_device_id", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("pred_device_id", MySqlDbType.Int32, 6).Value = Int32.Parse(preddevidtxtbox.Text);
                        }

                        if (skipoutertxtbox.Text == "")
                        {
                            cmd.Parameters.Add("skip_outer", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("skip_outer", MySqlDbType.Int32, 6).Value = Int32.Parse(skipoutertxtbox.Text);

                        }

                        if (skipinnertxtbox.Text == "")
                        {
                            cmd.Parameters.Add("skip_inner", MySqlDbType.Int32, 6).Value = 0;

                        }

                        else
                        {
                            cmd.Parameters.Add("skip_inner", MySqlDbType.Int32, 6).Value = Int32.Parse(skipinnertxtbox.Text);

                        }

                        if (inittxtbox.Text == "")
                        {
                            cmd.Parameters.Add("init", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("init", MySqlDbType.Int32, 6).Value = Int32.Parse(inittxtbox.Text);

                        }

                        if (lasttxtbox.Text == "")
                        {
                            cmd.Parameters.Add("last", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("last", MySqlDbType.Int32, 6).Value = Int32.Parse(lasttxtbox.Text);

                        }

                        if (initvidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("initVideo", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("initVideo", MySqlDbType.Int32, 6).Value = Int32.Parse(initvidtxtbox.Text);

                        }

                        if (lastvidtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("lastVideo", MySqlDbType.Int32, 6).Value = 0;

                        }

                        else
                        {
                            cmd.Parameters.Add("lastVideo", MySqlDbType.Int32, 6).Value = Int32.Parse(lastvidtxtbox.Text);

                        }

                        if (procdurtxtbox.Text == "")
                        {
                            cmd.Parameters.Add("processDuration", MySqlDbType.Int32, 6).Value = 0;

                        }
                        else
                        {
                            cmd.Parameters.Add("processDuration", MySqlDbType.Int32, 6).Value = Int32.Parse(procdurtxtbox.Text);

                        }

                        cmd.Parameters.Add("det_model_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbdetmodelid.Text);
                        cmd.Parameters.Add("pred_model_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbpredmodelid.Text);
                        cmd.Parameters.Add("server_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbserverid.Text);
                        cmd.Parameters.Add("camera_id", MySqlDbType.Int32, 6).Value = Int32.Parse(cbbcameraid.Text);




                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(analyticsidtxtbox.Text);


                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }

        public void AUDstatistics(string sqlstatement, int state) //pause
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        try
                        {
                            msg = "New Row Inserted";


                            string date = DateTime.UtcNow.ToString("dd-MMM-yyyy");

                            string time = DateTime.Now.ToString("HH:mm:ss ");
                            cmd.Parameters.Add("analytic_id", MySqlDbType.Int32, 6).Value = Int32.Parse(comboboxanalyticidstat.Text);
                            cmd.Parameters.Add("date", MySqlDbType.VarChar).Value = date;
                            cmd.Parameters.Add("time", MySqlDbType.VarChar).Value = time;

                            cmd.Parameters.Add("total_vehicle", MySqlDbType.Int32, 6).Value = Int32.Parse(totalvehiclestatstxtbox.Text);
                            cmd.Parameters.Add("vehi_1", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi1stats.Text);
                            cmd.Parameters.Add("vehi_2", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi2stats.Text);
                            cmd.Parameters.Add("vehi_3", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi3stats.Text);
                            cmd.Parameters.Add("vehi_4", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi4stats.Text);
                            cmd.Parameters.Add("vehi_5", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi5stats.Text);
                            cmd.Parameters.Add("vehi_6", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi6stats.Text);

                            cmd.Parameters.Add("avg_vehicle_gap", MySqlDbType.Decimal).Value = Decimal.Parse(avggapstatstxtbox.Text);
                            cmd.Parameters.Add("avg_vehicle_speed", MySqlDbType.Decimal).Value = Decimal.Parse(avgspeedstatstxtbox.Text);

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                        }

                    }
                    break;
                case 1:
                    {

                        msg = "Row Updated";
                        string date = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                        string time = DateTime.Now.ToString("hh:mm:ss tt");
                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(statisticsidtxtbox.Text);
                        cmd.Parameters.Add("analytic_id", MySqlDbType.Int32, 6).Value = Int32.Parse(comboboxanalyticidstat.Text);
                        cmd.Parameters.Add("date", MySqlDbType.VarChar).Value = date;
                        cmd.Parameters.Add("time", MySqlDbType.VarChar).Value = time;

                        cmd.Parameters.Add("total_vehicle", MySqlDbType.Int32, 6).Value = Int32.Parse(totalvehiclestatstxtbox.Text);
                        cmd.Parameters.Add("vehi_1", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi1stats.Text);
                        cmd.Parameters.Add("vehi_2", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi2stats.Text);
                        cmd.Parameters.Add("vehi_3", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi3stats.Text);
                        cmd.Parameters.Add("vehi_4", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi4stats.Text);
                        cmd.Parameters.Add("vehi_5", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi5stats.Text);
                        cmd.Parameters.Add("vehi_6", MySqlDbType.Int32, 6).Value = Int32.Parse(vehi6stats.Text);

                        cmd.Parameters.Add("avg_vehicle_gap", MySqlDbType.Decimal).Value = Decimal.Parse(avggapstatstxtbox.Text);
                        cmd.Parameters.Add("avg_vehicle_speed", MySqlDbType.Decimal).Value = Decimal.Parse(avgspeedstatstxtbox.Text);



                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("id", MySqlDbType.Int32, 6).Value = Int32.Parse(statisticsidtxtbox.Text);


                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void AUDpredictionmodule(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        try
                        {
                            msg = "New Row Inserted";

                            cmd.Parameters.Add("pred_framework", MySqlDbType.VarChar).Value = predframeworktxtbox.Text;
                            cmd.Parameters.Add("pred_source", MySqlDbType.VarChar).Value = predsourcetxtbox.Text;
                            cmd.Parameters.Add("pred_size", MySqlDbType.VarChar).Value = predsizetxtbox.Text;
                            cmd.Parameters.Add("pred_model", MySqlDbType.VarChar).Value = predmodeltxbox.Text;
                            cmd.Parameters.Add("pred_label", MySqlDbType.VarChar).Value = predlabeltxtbox.Text;
                            cmd.Parameters.Add("pred_labelmap", MySqlDbType.VarChar).Value = predlabelmappertxtbox.Text;
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    break;
                case 1:
                    {

                        msg = "Row Updated";
                        cmd.Parameters.Add("pred_id", MySqlDbType.Int32, 6).Value = Int32.Parse(predidtxtbox.Text);
                        cmd.Parameters.Add("pred_framework", MySqlDbType.VarChar).Value = predframeworktxtbox.Text;
                        cmd.Parameters.Add("pred_source", MySqlDbType.VarChar).Value = predsourcetxtbox.Text;
                        cmd.Parameters.Add("pred_size", MySqlDbType.VarChar).Value = predsizetxtbox.Text;
                        cmd.Parameters.Add("pred_model", MySqlDbType.VarChar).Value = predmodeltxbox.Text;
                        cmd.Parameters.Add("pred_label", MySqlDbType.VarChar).Value = predlabeltxtbox.Text;
                        cmd.Parameters.Add("pred_labelmap", MySqlDbType.VarChar).Value = predlabelmappertxtbox.Text;

                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("pred_id", MySqlDbType.Int32, 6).Value = Int32.Parse(predidtxtbox.Text);


                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }

        public void AUDdetectionmodule(string sqlstatement, int state)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            string msg = "";
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlstatement;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    {
                        try
                        {
                            msg = "New Row Inserted";
                            //cmd.Parameters.Add("det_id", MySqlDbType.Int32, 6).Value = Int32.Parse(detidtxtbox.Text);
                            cmd.Parameters.Add("det_framework", MySqlDbType.VarChar).Value = detframeworktxtbox.Text;
                            cmd.Parameters.Add("det_source", MySqlDbType.VarChar).Value = detsourcetxtbox.Text;
                            cmd.Parameters.Add("det_size", MySqlDbType.VarChar).Value = detsizetxtbox.Text;
                            cmd.Parameters.Add("det_model", MySqlDbType.VarChar).Value = detmodeltxtbox.Text;
                            cmd.Parameters.Add("det_label", MySqlDbType.VarChar).Value = detlabeltxtbox.Text;
                            cmd.Parameters.Add("det_labelmap", MySqlDbType.VarChar).Value = detlabelmappertxtbox.Text;
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    break;
                case 1:
                    {

                        msg = "Row Updated";
                        cmd.Parameters.Add("det_id", MySqlDbType.Int32, 6).Value = Int32.Parse(detidtxtbox.Text);
                        cmd.Parameters.Add("det_framework", MySqlDbType.VarChar).Value = detframeworktxtbox.Text;
                        cmd.Parameters.Add("det_source", MySqlDbType.VarChar).Value = detsourcetxtbox.Text;
                        cmd.Parameters.Add("det_size", MySqlDbType.VarChar).Value = detsizetxtbox.Text;
                        cmd.Parameters.Add("det_model", MySqlDbType.VarChar).Value = detmodeltxtbox.Text;
                        cmd.Parameters.Add("det_label", MySqlDbType.VarChar).Value = detlabeltxtbox.Text;
                        cmd.Parameters.Add("det_labelmap", MySqlDbType.VarChar).Value = detlabelmappertxtbox.Text;



                    }
                    break;
                case 2:
                    {
                        msg = "Deleted Selected Row";
                        cmd.Parameters.Add("det_id", MySqlDbType.Int32, 6).Value = Int32.Parse(detidtxtbox.Text);


                    }
                    break;
            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updatealldatagrid();
                    con.Close();
                }

            }
            catch (Exception ex)
            {

            }

        }

        #endregion
        //AUD region end

        private void updatealldatagrid()
        {
            FillGrid("SELECT id,ip_address FROM server_info", serverinfodatagrid, "server_info");
            FillGrid("SELECT cam_id,cam_source,cam_url,cam_mask,cam_ext FROM camera", cameradatagrid, "camera");
            FillGrid("SELECT id,source FROM input_source", inputsourcedatagrid, "input_source");
            FillGrid("SELECT id,analytic_id,date,time,total_vehicle,vehi_1,vehi_2," +
                "vehi_3,vehi_4,vehi_5,vehi_6,avg_vehicle_speed," +
                "avg_vehicle_gap FROM statistics", statisticdatagrid, "statistics");
            FillGrid("SELECT id,det_conf,det_debug,trk_debug," +
                "pred_enable,det_device_id,pred_device_id,skip_outer,skip_inner," +
                "init,last,initVideo,lastVideo,processDuration,status,det_model_id," +
                "pred_model_id,server_id,camera_id FROM analytic", analyticdatagrid, "analytic");
            FillGrid("SELECT pred_id,pred_framework,pred_source, pred_size, pred_model, pred_label, pred_labelmap" +
                " FROM prediction", predictionmoduledatagrid, "prediction");
            FillGrid("SELECT det_id,det_framework,det_source, det_size, det_model, det_label, det_labelmap" +
                " FROM detection", detectionmoduledatagrid, "detection");

            bindcomboboxanalyticidstatistic();
            bindcomboboxanalyticidstatistic(cbbanalyticfiltersearch);
            //bindcomboboxcamidstat();
            bindcbbdetmodelidanalytics();
            bindcbbserveridanalytics();
            bindcbbcamidanalytics();
            bindcbbpredmodelidanalytics();


        }

        #region //selectionchanged region


        private void serverinfodatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {
                serverinfoidtextbox.Text = drv["id"].ToString();
                serverinfoiptextbox.Text = drv["ip_address"].ToString();
            }
        }

        private void inputsourcedatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {
                inputsourceidtextbox.Text = drv["id"].ToString();
                inputsourcesourcetextbox.Text = drv["source"].ToString();
            }

        }


        private void cameradatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  resetallfieldparameter();
            string sourceoption;
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {

                sourceoption = drv["cam_source"].ToString();

                if (sourceoption == "1")
                {
                    comboboxsourceoption.SelectedIndex = 0;
                    cameraidtxtbox.Text = drv["cam_id"].ToString();
                    //imgfoldertxtbox.Text = drv["cam_url"].ToString();
                    string url = drv["cam_url"].ToString();
                    string newurl = url.Replace(@"\\", @"\");
                    imgfoldertxtbox.Text = newurl;
                    filesextensiontxtbox.Text = drv["cam_ext"].ToString();
                    videofoldertxtbox.Text = "";
                    cameraurltxtbox.Text = "";
                    cameramasktxtbox.Text = drv["cam_mask"].ToString();
                }
                else if (sourceoption == "2")
                {
                    cameraidtxtbox.Text = drv["cam_id"].ToString();
                    comboboxsourceoption.SelectedIndex = 1;
                    string url = drv["cam_url"].ToString();
                    string newurl = url.Replace(@"\\", @"\");
                    videofoldertxtbox.Text = newurl;
                    cameraurltxtbox.Text = "";
                    cameramasktxtbox.Text = drv["cam_mask"].ToString();
                    filesextensiontxtbox.Text = drv["cam_ext"].ToString();
                }

                else if (sourceoption == "3")
                {
                    cameraidtxtbox.Text = drv["cam_id"].ToString();
                    comboboxsourceoption.SelectedIndex = 2;
                    string url = drv["cam_url"].ToString();
                    string newurl = url.Replace(@"\\", @"\");
                    cameraurltxtbox.Text = newurl;
                    videofoldertxtbox.Text = "";
                    cameramasktxtbox.Text = drv["cam_mask"].ToString();
                    filesextensiontxtbox.Text = "";
                }
                else
                {
                    cameraidtxtbox.Text = "";
                    cameramasktxtbox.Text = "";
                    cameraurltxtbox.Text = "";
                    imgfoldertxtbox.Text = "";
                    videofoldertxtbox.Text = "";
                    comboboxsourceoption.Text = "";
                    filesextensiontxtbox.Text = "";
                }






            }

        }

        private void statisticdatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   resetallfieldparameter();
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {
                string analyticid = drv["analytic_id"].ToString();
                //string camid = drv["camera_id"].ToString();

                if (analyticid == "")
                {
                    comboboxanalyticidstat.Text = "";
                }
                else
                {

                    comboboxanalyticidstat.Text = analyticid;
                }

              

                statisticsidtxtbox.Text = drv["id"].ToString();
                //analyticidstatstxtbox.Text = drv["analytic_id"].ToString();
                statsdatetxtbox.Text = drv["date"].ToString();
                timestatstxtbox.Text = drv["time"].ToString();
               
                // camerastatstxtbox.Text = drv["camera_id"].ToString();
                totalvehiclestatstxtbox.Text = drv["total_vehicle"].ToString();
                vehi1stats.Text = drv["vehi_1"].ToString();
                vehi2stats.Text = drv["vehi_2"].ToString();
                vehi3stats.Text = drv["vehi_3"].ToString();
                vehi4stats.Text = drv["vehi_4"].ToString();
                vehi5stats.Text = drv["vehi_5"].ToString();
                vehi6stats.Text = drv["vehi_6"].ToString();
                avgspeedstatstxtbox.Text = drv["avg_vehicle_speed"].ToString();
                avggapstatstxtbox.Text = drv["avg_vehicle_gap"].ToString();



            }


        }

        private void analyticdatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // resetallfieldparameter();
            string radio1 = "False";
            string radio2 = "False";
            string radio3 = "False";
            string radio4 = "False";
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {

                analyticsidtxtbox.Text = drv["id"].ToString();

                string detconf = drv["det_conf"].ToString();
                det_confSlider.Value = Convert.ToDouble(detconf);

                detdevidtxtbox.Text = drv["det_device_id"].ToString();
                preddevidtxtbox.Text = drv["pred_device_id"].ToString();
                skipoutertxtbox.Text = drv["skip_outer"].ToString();
                skipinnertxtbox.Text = drv["skip_inner"].ToString();
                inittxtbox.Text = drv["init"].ToString();
                lasttxtbox.Text = drv["last"].ToString();
                initvidtxtbox.Text = drv["initVideo"].ToString();
                lastvidtxtbox.Text = drv["lastVideo"].ToString();
                procdurtxtbox.Text = drv["processDuration"].ToString();


                string detmodelid = drv["det_model_id"].ToString();

                if (detmodelid == "")
                {
                    cbbdetmodelid.Text = "";

                }
                else
                {
                    cbbdetmodelid.Text = detmodelid;
                }


                string predmodelid = drv["pred_model_id"].ToString();

                if (detmodelid == "")
                {
                    cbbpredmodelid.Text = "";

                }
                else
                {
                    cbbpredmodelid.Text = predmodelid;
                }


                string serverid = drv["server_id"].ToString();

                if (serverid == "")
                {
                    cbbserverid.Text = "";

                }
                else
                {
                    cbbserverid.Text = serverid;
                }


                string cameraid = drv["camera_id"].ToString();

                if (cameraid == "")
                {
                    cbbcameraid.Text = "";

                }
                else
                {
                    cbbcameraid.Text = cameraid;
                }


                radio1 = drv["det_debug"].ToString();
                radio2 = drv["trk_debug"].ToString();
                radio3 = drv["pred_enable"].ToString();
                radio4 = drv["status"].ToString();

                detdebug = radio1;
                trkdebug = radio2;
                predenable = radio3;

                if (radio1 == "True")
                {
                    det_debugtrue.IsChecked = true;
                    det_debugfalse.IsChecked = false;


                }
                else
                {
                    det_debugfalse.IsChecked = true;
                    det_debugtrue.IsChecked = false;
                }

                if (radio2 == "True")
                {
                    trkdebugtrue.IsChecked = true;
                    trkdebugfalse.IsChecked = false;


                }
                else
                {
                    trkdebugtrue.IsChecked = false;
                    trkdebugfalse.IsChecked = true;
                }

                if (radio3 == "True")
                {
                    predenabletrue.IsChecked = true;
                    predenablefalse.IsChecked = false;


                }
                else
                {
                    predenabletrue.IsChecked = false;
                    predenablefalse.IsChecked = true;

                }

                if (radio4 == "True")
                {
                    statustrue.IsChecked = true;
                    statusfalse.IsChecked = false;


                }
                else
                {
                    statustrue.IsChecked = false;
                    statusfalse.IsChecked = true;
                }








            }

        }



        private void predictionmoduledatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //resetallfieldparameter();
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {


                predidtxtbox.Text = drv["pred_id"].ToString();
                predframeworktxtbox.Text = drv["pred_framework"].ToString();
                predsourcetxtbox.Text = drv["pred_source"].ToString();
                predsizetxtbox.Text = drv["pred_size"].ToString();
                predmodeltxbox.Text = drv["pred_model"].ToString();
                predlabeltxtbox.Text = drv["pred_label"].ToString();
                predlabelmappertxtbox.Text = drv["pred_labelmap"].ToString();
            }

        }

        private void detectionmoduledatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //resetallfieldparameter();
            DataGrid dg = sender as DataGrid;
            DataRowView drv = dg.SelectedItem as DataRowView;
            if (drv != null)
            {
                detidtxtbox.Text = drv["det_id"].ToString();
                detframeworktxtbox.Text = drv["det_framework"].ToString();
                detsourcetxtbox.Text = drv["det_source"].ToString();
                detsizetxtbox.Text = drv["det_size"].ToString();
                detmodeltxtbox.Text = drv["det_model"].ToString();
                detlabeltxtbox.Text = drv["det_label"].ToString();
                detlabelmappertxtbox.Text = drv["det_labelmap"].ToString();
            }

        }

        #endregion
        //end region SELECTION CHANGED

        private void resetallfieldbuttonparameterbuttong_Click(object sender, RoutedEventArgs e)
        {
            resetallfieldparameter();
        }

        private void updatedatabasebutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (tabcontrolparameter.SelectedIndex)
                {
                    case 0:  //done
                        {
                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        try
                                        {
                                            string sqlstatement = "UPDATE server_info SET ip_address ='" + serverinfoiptextbox.Text + "' WHERE" +
                                               " id = " + Convert.ToInt16(serverinfoidtextbox.Text) + " ;";

                                            this.AUDserverinfo(sqlstatement, 1);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row to update");

                                        }
                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;

                    case 1:  //done
                        {
                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        string sqlstatement;

                                        try
                                        {
                                            if (comboboxsourceoption.SelectedIndex == 0)
                                            {
                                                sqlstatement = "UPDATE camera SET cam_source= 1, cam_url = \"" + imgfoldertxtbox.Text + "\", cam_mask = \"" + cameramasktxtbox.Text + "\", cam_ext = \"" + filesextensiontxtbox.Text + "\" WHERE cam_id = " + Convert.ToInt16(cameraidtxtbox.Text) + " ;";

                                                this.AUDserverinfo(sqlstatement, 1);
                                            }

                                            else if (comboboxsourceoption.SelectedIndex == 1)
                                            {
                                                sqlstatement = "UPDATE camera SET cam_source= 2, cam_url = \"" + videofoldertxtbox.Text + "\", cam_mask = \"" + cameramasktxtbox.Text + "\", cam_ext = \"" + filesextensiontxtbox.Text + "\" WHERE cam_id = " + Convert.ToInt16(cameraidtxtbox.Text) + " ;";

                                                this.AUDserverinfo(sqlstatement, 1);
                                            }

                                            else if (comboboxsourceoption.SelectedIndex == 2)
                                            {
                                                sqlstatement = "UPDATE camera SET cam_source= 3, cam_url = \"" + cameraurltxtbox.Text + "\", cam_mask = \"" + cameramasktxtbox.Text + "\" WHERE cam_id = " + Convert.ToInt16(cameraidtxtbox.Text) + " ;";

                                                this.AUDserverinfo(sqlstatement, 1);
                                            }

                                            else if (cameraidtxtbox.Text == "")
                                            {
                                                MessageBox.Show("Invalid Row / Select a row first");
                                            }
                                        }

                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row first");
                                        }

                                        try
                                        {
                                            // Check if file exists with its full path    
                                            if (File.Exists("Roi Points.txt"))
                                            {
                                                // If file found, delete it    
                                                File.Delete("Roi Points.txt");

                                            }
                                            else Debug.WriteLine("Roi Points.txt not found");

                                        }
                                        catch (IOException ioExp)
                                        {
                                            MessageBox.Show(ioExp.Message);
                                        }



                                        //this.AUDserverinfo(sqlstatement, 1);
                                    }

                                    break;

                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;
                    case 2:  //done
                        {

                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "UPDATE input_source SET source =\"" + inputsourcesourcetextbox.Text + "\" WHERE id = " + Convert.ToInt16(inputsourceidtextbox.Text) + ";";

                                        this.AUDinputsource(sqlstatement, 1);
                                    }

                                    break;
                                case MessageBoxResult.No:

                                    break;
                            }


                        }
                        break;
                    case 3: //done
                        {
                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "UPDATE analytic SET det_conf= @det_conf, det_debug= @det_debug, trk_debug= @trk_debug, pred_enable = @pred_enable, status= @status, det_device_id = @det_device_id, pred_device_id = @pred_device_id, skip_outer = @skip_outer, skip_inner= @skip_inner, init =@init ,last= @last,initVideo =@initVideo,lastVideo =@lastVideo, processDuration =@processDuration, det_model_id =@det_model_id, pred_model_id =@pred_model_id, server_id= @server_id, camera_id=@camera_id WHERE id =@id;";

                                        this.AUDanalytics(sqlstatement, 1);
                                    }

                                    break;
                                case MessageBoxResult.No:

                                    break;
                            }
                        }
                        break;
                    case 4: //done
                        {
                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        try
                                        {
                                            string sqlstatement = "UPDATE statistics SET analytic_id = @analytic_id, date=@date, time=@time,total_vehicle=@total_vehicle, vehi_1=@vehi_1, vehi_2 =@vehi_2, vehi_3 =@vehi_3, vehi_4 =@vehi_4, vehi_5 =@vehi_5, vehi_6 =@vehi_6, avg_vehicle_speed =@avg_vehicle_speed, avg_vehicle_gap=@avg_vehicle_gap   WHERE id=@id";

                                            this.AUDstatistics(sqlstatement, 1);
                                        }

                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row first");
                                        }
                                    }

                                    break;
                                case MessageBoxResult.No:

                                    break;
                            }

                        }
                        break;
                    case 5: //done
                        {

                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        try
                                        {
                                            string sqlstatement = "UPDATE prediction SET pred_framework = \"" + predframeworktxtbox.Text + "\", pred_source=\"" + predsourcetxtbox.Text + "\",pred_size=\"" + predsizetxtbox.Text + "\",pred_model=\"" + predmodeltxbox.Text + "\",pred_label=\"" + predlabeltxtbox.Text + "\",pred_labelmap=\"" + predlabelmappertxtbox.Text + "\" WHERE pred_id =" + Convert.ToInt16(predidtxtbox.Text) + ";";

                                            this.AUDpredictionmodule(sqlstatement, 1);
                                        }

                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row first");
                                        }
                                    }

                                    break;
                                case MessageBoxResult.No:

                                    break;
                            }

                        }
                        break;

                    case 6: //done
                        {

                            MessageBoxResult result = MessageBox.Show("Confirm Update values?", "Update Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        try
                                        {
                                            // string sqlstatement = "UPDATE prediction SET pred_framework = \"" + predframeworktxtbox.Text + "\", pred_source=\"" + predsourcetxtbox.Text + "\",pred_size=\"" + predsizetxtbox.Text + "\",pred_model=\"" + predmodeltxbox.Text + "\",pred_label=\"" + predlabeltxtbox.Text + "\",pred_labelmap=\"" + predlabelmappertxtbox.Text + "\" WHERE pred_id =" + Convert.ToInt16(predidtxtbox.Text) + ";";
                                            string sqlstatement = "UPDATE detection SET det_framework =@det_framework, det_source =@det_source, det_size=@det_size, det_model=@det_model, det_label=@det_label, det_labelmap=@det_labelmap WHERE det_id =@det_id ;";

                                            this.AUDdetectionmodule(sqlstatement, 1);
                                        }

                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row first");
                                        }
                                    }

                                    break;
                                case MessageBoxResult.No:

                                    break;
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select a row to update first");
            }
        }

        private void deleterowbuttondb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (tabcontrolparameter.SelectedIndex)
                {
                    case 0: //done
                        {
                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {


                                        try
                                        {


                                            string sqlstatement = "DELETE FROM server_info WHERE id = @id;";


                                            this.AUDserverinfo(sqlstatement, 2);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row to delete first");
                                        }


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;

                    case 1: //done
                        {

                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        try
                                        {

                                            string sqlstatement = "DELETE FROM camera WHERE cam_id =@cam_id;";

                                            this.AUDcamera(sqlstatement, 2);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Select a row to delete first");
                                        }


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;
                    case 2: //done
                        {

                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "DELETE FROM input_source WHERE id = " + Convert.ToInt16(inputsourceidtextbox.Text) + ";";


                                        this.AUDinputsource(sqlstatement, 2);


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }


                        }
                        break;
                    case 3: //done
                        {
                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "DELETE FROM analytic WHERE id = @id;";


                                        this.AUDanalytics(sqlstatement, 2);


                                    }

                                    break;

                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;
                    case 4://done
                        {
                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "DELETE FROM statistics WHERE id = @id;";


                                        this.AUDstatistics(sqlstatement, 2);


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;
                    case 5: //done
                        {
                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "DELETE FROM prediction WHERE pred_id = " + Convert.ToInt16(predidtxtbox.Text) + ";";


                                        this.AUDpredictionmodule(sqlstatement, 2);


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;

                    case 6: //done
                        {

                            MessageBoxResult result = MessageBox.Show("Delete this row?", "Delete Row", MessageBoxButton.YesNo);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {

                                        string sqlstatement = "DELETE FROM detection WHERE det_id =@det_id ;";


                                        this.AUDdetectionmodule(sqlstatement, 2);


                                    }

                                    break;
                                case MessageBoxResult.Cancel:

                                    break;
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select a row to delete first");
            }
        }

        private void resetallfieldparameter()
        {
            comboboxsourceoption.Text = "";
            serverinfoidtextbox.Text = "";
            serverinfoiptextbox.Text = "";
            inputsourceidtextbox.Text = "";
            inputsourcesourcetextbox.Text = "";
            cameramasktxtbox.Text = "";
            cameraurltxtbox.Text = "";
            imgfoldertxtbox.Text = "";
            videofoldertxtbox.Text = "";
            comboboxsourceoption.Text = "";
            filesextensiontxtbox.Text = "";
            statisticsidtxtbox.Text = "";
            //analyticidstatstxtbox.Text = "";
            statsdatetxtbox.Text = "";
            timestatstxtbox.Text = "";

            //camerastatstxtbox.Text = "";
            totalvehiclestatstxtbox.Text = "0";
            vehi1stats.Text = "0";
            vehi2stats.Text = "0";
            vehi3stats.Text = "0";
            vehi4stats.Text = "0";
            vehi5stats.Text = "0";
            vehi6stats.Text = "0";
            avgspeedstatstxtbox.Text = "0";
            avggapstatstxtbox.Text = "0";
            analyticsidtxtbox.Text = "";
            detdevidtxtbox.Text = "";
            preddevidtxtbox.Text = "";
            skipoutertxtbox.Text = "";
            skipinnertxtbox.Text = "";
            inittxtbox.Text = "";
            lasttxtbox.Text = "";
            initvidtxtbox.Text = "";
            lastvidtxtbox.Text = "";
            procdurtxtbox.Text = "";

            det_debugtrue.IsChecked = false;
            det_debugfalse.IsChecked = false;
            trkdebugtrue.IsChecked = false;
            trkdebugfalse.IsChecked = false;
            predenabletrue.IsChecked = false;
            predenablefalse.IsChecked = false;
            statustrue.IsChecked = false;
            statusfalse.IsChecked = false;
            predidtxtbox.Text = "";
            predsourcetxtbox.Text = "";
            predsizetxtbox.Text = "";
            predframeworktxtbox.Text = "";
            predlabeltxtbox.Text = "";
            predlabelmappertxtbox.Text = "";
            predmodeltxbox.Text = "";
            detidtxtbox.Text = "";
            detframeworktxtbox.Text = "";
            detsizetxtbox.Text = "";
            detsourcetxtbox.Text = "";
            detlabeltxtbox.Text = "";
            detlabelmappertxtbox.Text = "";
            detmodeltxtbox.Text = "";

            cbbcameraid.Text = "";
            cbbdetmodelid.Text = "";
            cbbpredmodelid.Text = "";
            cbbserverid.Text = "";
            comboboxanalyticidstat.Text = "";

            det_confSlider.Value = 0;
            cameraidtxtbox.Text = "";
            comboboxsourceoption.Text = "--Select Source--";


        }

        public void bindcomboboxanalyticidstatistic()
        {

            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from analytic", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboboxanalyticidstat.ItemsSource = dt.DefaultView;
                comboboxanalyticidstat.DisplayMemberPath = "id";
                comboboxanalyticidstat.SelectedValuePath = "id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void bindcomboboxanalyticidstatistic(ComboBox cbb)
        {

            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from analytic", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbb.ItemsSource = dt.DefaultView;
                cbb.DisplayMemberPath = "id";
                cbb.SelectedValuePath = "id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

     /*   public void bindcomboboxcamidstat()
        {

            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from camera", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboboxcameraidstatt.ItemsSource = dt.DefaultView;
                comboboxcameraidstatt.DisplayMemberPath = "cam_id";
                comboboxcameraidstatt.SelectedValuePath = "cam_id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        public void bindcbbdetmodelidanalytics()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from detection", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbbdetmodelid.ItemsSource = dt.DefaultView;
                cbbdetmodelid.DisplayMemberPath = "det_id";
                cbbdetmodelid.SelectedValuePath = "det_id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void bindcbbpredmodelidanalytics()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from prediction", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbbpredmodelid.ItemsSource = dt.DefaultView;
                cbbpredmodelid.DisplayMemberPath = "pred_id";
                cbbpredmodelid.SelectedValuePath = "pred_id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void bindcbbserveridanalytics()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from server_info", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbbserverid.ItemsSource = dt.DefaultView;
                cbbserverid.DisplayMemberPath = "id";
                cbbserverid.SelectedValuePath = "id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void bindcbbcamidanalytics()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;

            try
            {
                MySqlConnection con = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = new MySqlCommand("select * from camera", con);
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbbcameraid.ItemsSource = dt.DefaultView;
                cbbcameraid.DisplayMemberPath = "cam_id";
                cbbcameraid.SelectedValuePath = "cam_id";
                cmd.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void totalvehiclestatstxtbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void Exportscriptbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result1 = MessageBox.Show("Confirm export?", "Export into Script Command", MessageBoxButton.YesNo);
                switch (result1)
                {
                    case MessageBoxResult.Yes:
                        {
                            if (cbbpredmodelid.Text == "" || cbbdetmodelid.Text == "" || cbbcameraid.Text == "")
                            {
                                MessageBox.Show("Please select a row first on the datagrid box");

                            }
                            else
                            {


                                getvaluefromdet();
                                getvaluefrompred();
                                geturlfromcam();

                                string newmask = camitems[2];
                                newmask = newmask.Replace("\r\n", "");

                                string trimmedmask = newmask + ".txt";
                                string text2write = camitems[2];
                                Debug.WriteLine(camitems[2]);

                                trimmedmask = trimmedmask.Replace("\r\n", string.Empty);

                                string roiscript;

                                if (camitems[0] == "1")
                                {
                                    roiscript = "OfflineTest.exe --img=\"" + camitems[1] + "\"  --ext=\"mp4\" --mask=" + trimmedmask + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" + " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;
                                }

                                else if (camitems[0] == "2")
                                {
                                    roiscript = "OfflineTest.exe --vid=\"" + camitems[1] + "\"  --ext=\"mp4\" --mask=" + trimmedmask + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" + " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;
                                }

                                else
                                {

                                    roiscript = "OfflineTest.exe --cam=\"" + camitems[1] + "\"  --ext=\"\" --mask=" + trimmedmask + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" + " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;

                                }
                                
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.InitialDirectory = @"C:\";
                                sfd.RestoreDirectory = true;
                                sfd.FileName = "*.cmd";
                                sfd.DefaultExt = "cmd";
                                sfd.Filter = "cmd files (*.cmd)|*.cmd";

                                if(sfd.ShowDialog() == true)
                                {
                                    File.WriteAllText(sfd.FileName, roiscript);
                                    
                                }


                                string savedirectory = System.IO.Path.GetDirectoryName(sfd.FileName);

                                System.IO.StreamWriter writer = new System.IO.StreamWriter(savedirectory+"\\" + trimmedmask);
                                writer.Write(text2write);
                                writer.Close();
                                /*
                                Winforms.FolderBrowserDialog FolderDialog = new Winforms.FolderBrowserDialog();
                                FolderDialog.ShowNewFolderButton = false;
                                FolderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
                                Winforms.DialogResult result = FolderDialog.ShowDialog();


                                if (result == Winforms.DialogResult.OK)
                                {
                                    string fpath = FolderDialog.SelectedPath;
                                    scriptpath = fpath;
                                    DirectoryInfo folder = new DirectoryInfo(fpath);


                                    if (folder.Exists)
                                    {

                                        string chars = "2346789ABCDEFGHJKLMNPQRTUVWXYZabcdefghjkmnpqrtuvwxyz";
                                        // create random generator
                                        Random rnd = new Random();
                                        string name;

                                        name = string.Empty;
                                        while (name.Length < 5)
                                        {
                                            name += chars.Substring(rnd.Next(chars.Length), 1);
                                        }
                                        // add extension
                                        name += ".cmd";
                                        // check against files in the folder

                                        File.WriteAllText(fpath + "\\demoscript-" + name, roiscript);

                                        System.IO.StreamWriter writer = new System.IO.StreamWriter(fpath + "\\" + trimmedmask);
                                        writer.Write(text2write);
                                        writer.Close();


                                        MessageBox.Show("Succesfully export script: demoscript-" + name + " and mask .txt file to target path");


                                    }


                                }*/
                                MessageBox.Show("Succesfully export script");

                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }





        }

        private void getvaluefrompred()
        {

            string longchar;


            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            // SqlConnection connection = new SqlConnection("Your Connection String Here");
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
            MySqlCommand command = new MySqlCommand("SELECT pred_framework,pred_source,pred_size,pred_model,pred_label,pred_labelmap FROM prediction WHERE pred_id=" + Int32.Parse(cbbpredmodelid.Text) + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString() + "|";
                }

                ListOfColumns = ListOfColumns + System.Environment.NewLine;
            }
            Debug.WriteLine(ListOfColumns);
            longchar = ListOfColumns;
            Debug.WriteLine(longchar);


            string[] longcharsplit = longchar.Split('|');
            for (int i = 0; i < longcharsplit.Length; i++)
            {
                predmodelitems[i] = longcharsplit[i];

            }
            connection.Close();





        }

        private void getvaluefromdet()
        {
            string longchar;

            
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            // SqlConnection connection = new SqlConnection("Your Connection String Here");
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
            MySqlCommand command = new MySqlCommand("SELECT det_framework,det_source,det_size,det_model,det_label,det_labelmap FROM detection WHERE det_id=" + Int32.Parse(cbbdetmodelid.Text) + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString() + "|";
                }

                ListOfColumns = ListOfColumns + System.Environment.NewLine;
            }
            Debug.WriteLine(ListOfColumns);
            longchar = ListOfColumns;
            Debug.WriteLine(longchar);


            string[] longcharsplit = longchar.Split('|');
            for (int i = 0; i < longcharsplit.Length; i++)
            {
                detmodelitems[i] = longcharsplit[i];

            }
            connection.Close();
        }

        private void geturlfromcam()
        {
            string longchar;


            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            // SqlConnection connection = new SqlConnection("Your Connection String Here");
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
            MySqlCommand command = new MySqlCommand("SELECT cam_source, cam_url, cam_mask, cam_ext FROM camera WHERE cam_id=" + Int32.Parse(cbbcameraid.Text) + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString() + "|";
                }

                ListOfColumns = ListOfColumns + System.Environment.NewLine;
            }
            Debug.WriteLine(ListOfColumns);
            longchar = ListOfColumns;
            Debug.WriteLine(longchar);


            string[] longcharsplit = longchar.Split('|');
            for (int i = 0; i < longcharsplit.Length; i++)
            {
                camitems[i] = longcharsplit[i];

            }
            connection.Close();

        }

        #region //get value for cartesian chart before and after filter


        private void getvaluetimespeedstats()
        {
            string longchar;


            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT time FROM systemapps.statistics WHERE date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' and analytic_id = 1 order by TIME(time) ASC;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();
                }

                ListOfColumns = ListOfColumns + System.Environment.NewLine;
            }
            longchar = ListOfColumns;

            string[] exampleArray = longchar.Split('\n');

            for (int i = 0; i < exampleArray.Length; i++)
            {
                exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
            }

            timespeedstatsarray = exampleArray;

            connection.Close();
        }

        private void getvaluetimespeedstats(string date, int analyticid, string timefrom, string timeto)
        {
            string longchar;


            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT time FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + " ORDER by TIME(time);", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();
                }

                ListOfColumns = ListOfColumns + System.Environment.NewLine;
            }
            longchar = ListOfColumns;

            string[] exampleArray = longchar.Split('\n');

            for (int i = 0; i < exampleArray.Length; i++)
            {
                exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
            }

            timespeedstatsarray = exampleArray;

            connection.Close();
        }

    
        private void getvaluespeedstats()
        {

            try
            {
                datedatafilterdashboardtext.Text = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                analyticidtextdashboard.Text = "1";
                string longchar;


                string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                // SqlConnection connection = new SqlConnection("Your Connection String Here");
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
                MySqlCommand command = new MySqlCommand("SELECT avg_vehicle_speed FROM systemapps.statistics WHERE date= '"+ DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' and analytic_id = 1 order by TIME(time) ASC;", connection);
                connection.Open();
                MySqlDataReader datareader = command.ExecuteReader();
                int ColumnCount = datareader.FieldCount;
                string ListOfColumns = string.Empty;
                while (datareader.Read())
                {
                    for (int i = 0; i <= ColumnCount - 1; i++)
                    {
                        ListOfColumns = ListOfColumns + datareader[i].ToString();
                    }

                    ListOfColumns = ListOfColumns + System.Environment.NewLine;
                }
                //Debug.WriteLine(ListOfColumns);
                longchar = ListOfColumns;

                string[] exampleArray = longchar.Split('\n');
                double[] valuespeed = new double[exampleArray.Length - 1];
                Debug.WriteLine(exampleArray.Length);



                for (int i = 0; i < exampleArray.Length; i++)
                {
                    exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
                }


                for (int i = 0; i < exampleArray.Length - 1; i++)
                {
                    valuespeed[i] = double.Parse(exampleArray[i]);
                }


                foreach (string value in exampleArray)
                {
                    Debug.WriteLine(value);
                }

                foreach (double value in valuespeed)
                {
                    Debug.WriteLine(value);
                }

                arrayspeedavg = valuespeed;





                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void getvaluespeedstats(string date, int analyticid,string timefrom,string timeto)
        {
            try
            {
                string longchar;


                string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                // SqlConnection connection = new SqlConnection("Your Connection String Here");
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
                MySqlCommand command = new MySqlCommand("SELECT avg_vehicle_speed FROM statistics WHERE TIME(time) BETWEEN TIME('"+timefrom+"') AND TIME('"+timeto+"') and date= '" + date+"' and analytic_id ="+analyticid+ " ORDER by TIME(time) ASC;", connection);
                connection.Open();
                MySqlDataReader datareader = command.ExecuteReader();
                int ColumnCount = datareader.FieldCount;
                string ListOfColumns = string.Empty;
                while (datareader.Read())
                {
                    for (int i = 0; i <= ColumnCount - 1; i++)
                    {
                        ListOfColumns = ListOfColumns + datareader[i].ToString();
                    }

                    ListOfColumns = ListOfColumns + System.Environment.NewLine;
                }
                //Debug.WriteLine(ListOfColumns);
                longchar = ListOfColumns;

                string[] exampleArray = longchar.Split('\n');
                double[] valuespeed = new double[exampleArray.Length - 1];
                Debug.WriteLine(exampleArray.Length);



                for (int i = 0; i < exampleArray.Length; i++)
                {
                    exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
                }


                for (int i = 0; i < exampleArray.Length - 1; i++)
                {
                    valuespeed[i] = double.Parse(exampleArray[i]);
                }


                foreach (string value in exampleArray)
                {
                    Debug.WriteLine(value);
                }

                foreach (double value in valuespeed)
                {
                    Debug.WriteLine(value);
                }

                arrayspeedavg = valuespeed;





                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    
        private void getvaluegapstats()
        {

            try
            {
                string longchar;


                string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                // SqlConnection connection = new SqlConnection("Your Connection String Here");
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
                MySqlCommand command = new MySqlCommand("SELECT avg_vehicle_speed FROM systemapps.statistics WHERE date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' and analytic_id = 1 order by TIME(time) ASC;", connection);
                connection.Open();
                MySqlDataReader datareader = command.ExecuteReader();
                int ColumnCount = datareader.FieldCount;
                string ListOfColumns = string.Empty;
                while (datareader.Read())
                {
                    for (int i = 0; i <= ColumnCount - 1; i++)
                    {
                        ListOfColumns = ListOfColumns + datareader[i].ToString();
                    }

                    ListOfColumns = ListOfColumns + System.Environment.NewLine;
                }
                //Debug.WriteLine(ListOfColumns);
                longchar = ListOfColumns;

                string[] exampleArray = longchar.Split('\n');
                double[] valuegap = new double[exampleArray.Length - 1];
                Debug.WriteLine(exampleArray.Length);



                for (int i = 0; i < exampleArray.Length; i++)
                {
                    exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
                }


                for (int i = 0; i < exampleArray.Length - 1; i++)
                {
                    valuegap[i] = double.Parse(exampleArray[i]);
                }


                foreach (string value in exampleArray)
                {
                    Debug.WriteLine(value);
                }

                foreach (double value in valuegap)
                {
                    Debug.WriteLine(value);
                }

                arraygapavg = valuegap;





                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void getvaluegapstats(string date, int analyticid, string timefrom, string timeto)
        {

            try
            {
                string longchar;


                string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
                // SqlConnection connection = new SqlConnection("Your Connection String Here");
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                // SqlCommand command = new SqlCommand("Your Select Statement Here",connection);
                MySqlCommand command = new MySqlCommand("SELECT avg_vehicle_gap FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + " ORDER BY TIME(time);", connection);
                connection.Open();
                MySqlDataReader datareader = command.ExecuteReader();
                int ColumnCount = datareader.FieldCount;
                string ListOfColumns = string.Empty;
                while (datareader.Read())
                {
                    for (int i = 0; i <= ColumnCount - 1; i++)
                    {
                        ListOfColumns = ListOfColumns + datareader[i].ToString();
                    }

                    ListOfColumns = ListOfColumns + System.Environment.NewLine;
                }
                //Debug.WriteLine(ListOfColumns);
                longchar = ListOfColumns;

                string[] exampleArray = longchar.Split('\n');
                double[] valuegap = new double[exampleArray.Length - 1];
                Debug.WriteLine(exampleArray.Length);



                for (int i = 0; i < exampleArray.Length; i++)
                {
                    exampleArray[i] = exampleArray[i].Replace("\n", "").Replace("\r", "");
                }


                for (int i = 0; i < exampleArray.Length - 1; i++)
                {
                    valuegap[i] = double.Parse(exampleArray[i]);
                }


                foreach (string value in exampleArray)
                {
                    Debug.WriteLine(value);
                }

                foreach (double value in valuegap)
                {
                    Debug.WriteLine(value);
                }

                arraygapavg = valuegap;





                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion
        //end region for cartesian chart before and after filter

        private void totalvehiclestatstxtbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (vehi1stats.Text == "" || vehi2stats.Text == "" || vehi3stats.Text == "" || vehi4stats.Text == "" || vehi5stats.Text == "" || vehi6stats.Text == "")
            {

            }
            else
            {
                int totalvehicle = Int32.Parse(vehi1stats.Text) + Int32.Parse(vehi2stats.Text) + Int32.Parse(vehi3stats.Text) + Int32.Parse(vehi4stats.Text) + Int32.Parse(vehi5stats.Text) + Int32.Parse(vehi6stats.Text);

                totalvehiclestatstxtbox.Text = totalvehicle.ToString();
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (vehi1stats.Text == "" || vehi2stats.Text == "" || vehi3stats.Text == "" || vehi4stats.Text == "" || vehi5stats.Text == "" || vehi6stats.Text == "")
            {

            }
            else
            {
                int totalvehicle = Int32.Parse(vehi1stats.Text) + Int32.Parse(vehi2stats.Text) + Int32.Parse(vehi3stats.Text) + Int32.Parse(vehi4stats.Text) + Int32.Parse(vehi5stats.Text) + Int32.Parse(vehi6stats.Text);

                totalvehiclestatstxtbox.Text = totalvehicle.ToString();
            }
        }

        private void Dbipaddresstestconn_TextChanged(object sender, TextChangedEventArgs e)
        {
            tabcontrolparameter.IsEnabled = false;
            packicondbconnectionstatus.Kind = PackIconKind.DatabaseRemove;
            packicondbconnectionstatus.Foreground = new SolidColorBrush(Colors.Red);
            isConnectionTested = false;
            resetallfieldparameter();
            cartchartdb.Visibility = Visibility.Hidden;
            speedchartcart.Visibility = Visibility.Hidden;
            dt.Stop();

        }

        private void refreshcart_Click(object sender, RoutedEventArgs e)
        {
            getvalueforgraphing();

            // SeriesCollection[0].Values.Clear();
            SeriesCollection[0].Values = arraygapavg.AsChartValues();
            Labels = timespeedstatsarray;

            SeriesCollection2[0].Values = arrayspeedavg.AsChartValues();
            Labels2 = timespeedstatsarray;

            cartchartdb.Update(true, true);
            speedchartcart.Update(true, true);

        }

        private void dothis()
        {
            try
            {
          
                SeriesCollection[0].Values = arraygapavg.AsChartValues();
                Labels = timespeedstatsarray;

                SeriesCollection2[0].Values = arrayspeedavg.AsChartValues();
                Labels2 = timespeedstatsarray;

                DataContext = Labels;
                DataContext = Labels2;

                totalvehiclechartmin.Value = totalvehiclemin;
                totalvehiclechartavg.Value = totalvehicleavg;
                totalvehiclechartmax.Value = totalvehiclemax;

                speedchartmin.Value = speedmin;
                speedchartavg.Value = speedavg;
                speedchartmax.Value = speedmax;

                gapchartmin.Value = gapmin;
                gapchartmax.Value = gapmax;
                gapchartavg.Value = gapavg;


                for (int i = 0; i < 6; i++)
                {
                    piechart1.Series[i].Values = new ChartValues<int> { vehicledistributionarray[i] };
                }


                DataContext = this;
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dothisafterfilter()
        {
            try
            {


                SeriesCollection[0].Values = arraygapavg.AsChartValues();
                Labels = timespeedstatsarray;

                SeriesCollection2[0].Values = arrayspeedavg.AsChartValues();
                Labels2 = timespeedstatsarray;

                DataContext = Labels;
                DataContext = Labels2;

                totalvehiclechartmin.Value = totalvehiclemin;
                totalvehiclechartavg.Value = totalvehicleavg;
                totalvehiclechartmax.Value = totalvehiclemax;

                speedchartmin.Value = speedmin;
                speedchartavg.Value = speedavg;
                speedchartmax.Value = speedmax;

                gapchartmin.Value = gapmin;
                gapchartmax.Value = gapmax;
                gapchartavg.Value = gapavg;




                DataContext = this;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
         
            dothis();
        }

        private void Dt2_Tick(object sender, EventArgs e)
        {
            getvalueafterfilter();
            dothisafterfilter();
        }


        private void pause_Click(object sender, RoutedEventArgs e)
        {
            dt.Stop();
        }

        private void autoref_Click(object sender, RoutedEventArgs e)
        {
            dothis();
            dt.Start();
        }

        private void disconnectandreset_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Windowmaximizebutton_Click(object sender, RoutedEventArgs e)
        {

            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }

            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void exportandrunscript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result1 = MessageBox.Show("Confirm export?", "Export into Script Command", MessageBoxButton.YesNo);
                switch (result1)
                {
                    case MessageBoxResult.Yes:
                        {
                            if (cbbpredmodelid.Text == "" || cbbdetmodelid.Text == "" || cbbcameraid.Text == "")
                            {
                                MessageBox.Show("Please select a row first on the datagrid box");

                            }
                            else
                            {


                                getvaluefromdet();
                                getvaluefrompred();
                                geturlfromcam();

                                string trimmedmask = camitems[2];
                                trimmedmask = trimmedmask.Replace("\r\n", "");

                                string path = trimmedmask + ".txt";
                                string text2write = camitems[2];
                                Debug.WriteLine(camitems[2]);

                                path = path.Replace("\r\n", string.Empty);




                                string roiscript;

                                if (camitems[0] == "1")
                                {
                                    roiscript = "OfflineTest.exe --img=\"" + camitems[1] + "\"  --ext=\"mp4\" --mask=" + path + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" + " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;

                                }

                                else if (camitems[0] == "2")
                                {
                                    roiscript = "OfflineTest.exe --vid=\"" + camitems[1] + "\"  --ext=\"mp4\" --mask=" + path + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" +  " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;
                                }

                                else
                                {

                                    roiscript = "OfflineTest.exe --cam=\"" + camitems[1] + "\"  --ext=\"\" --mask=" + path + " --label=vehicle2.txt --det_enable=true --det_deviceid=0 --det_framework=" + detmodelitems[0] + " --det_source=" + detmodelitems[1] + " --det_target=GPU --det_async=false --det_size=" + detmodelitems[3] + " --det_model=" + detmodelitems[2] + "  --det_input=data --det_label=" + detmodelitems[4] + " --det_labelmap=" + detmodelitems[5] + " --det_conf=" + det_conftxtblock.Text + " --det_debug=" + detdebug + "  --trk_loss=4 –-trk_debug=" + trkdebug + " --pre_enable=" + predenable + " --pre_deviceid=" + preddevidtxtbox.Text + " --pre_framework=" + predmodelitems[0] + " --pre_source=" + predmodelitems[1] + " --pre_target=GPU --pre_model=" + predmodelitems[2] + " --pre_input=data --pre_output=softmax --pre_outnum=6 --pre_label=" + predmodelitems[4] + " --pre_labelmap=" + predmodelitems[5] + " --pre_size=" + predmodelitems[3] + " --pre_batch=10 --pre_det=false --pre_conf=0.7 --pre_offset=0.0 --pre_boost=false --pre_overconf=0.5 --pre_topk=1 --pre_debug=false --init=" + inittxtbox.Text + " --last=" + lasttxtbox.Text + " --skip_outer=" + skipoutertxtbox.Text + " --skip_inner=" + skipinnertxtbox.Text + " --delay=1 --initVideo=" + initvidtxtbox.Text + " --lastVideo=" + lastvidtxtbox.Text + " --processDuration=" + procdurtxtbox.Text + " --count_line=55" + " --dbip=" + Dbipaddresstestconn.Text + " --dbname=" + Dbnametestconn.Text + " --dbusername=" + Dbusernametestconn.Text + " --dbpswd=" + Dbpasswordtestconn.Password;

                                }

                                

                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.InitialDirectory = @"C:\";
                                sfd.RestoreDirectory = true;
                                sfd.FileName = "*.cmd";
                                sfd.DefaultExt = "cmd";
                                sfd.Filter = "cmd files (*.cmd)|*.cmd";

                                if (sfd.ShowDialog() == true)
                                {
                                    File.WriteAllText(sfd.FileName, roiscript);
                                    FileInfo fi = new FileInfo(sfd.FileName);
                                    currentscriptname = fi.Name;

                                }
                                string savedfile = sfd.FileName;
                                string savedirectory = System.IO.Path.GetDirectoryName(sfd.FileName);

                                System.IO.StreamWriter writer = new System.IO.StreamWriter(savedirectory + "\\" + trimmedmask);
                                writer.Write(text2write);
                                writer.Close();

                                MessageBox.Show("Running script "+ currentscriptname);

                                //runscript previous generated script in command prompt

                               string strCmdText;
                               // strCmdText = "/K cd " + scriptpath + " & cd  & " + currentscriptname + " &  Echo Running Script.........  & timeout 10 & exit";
                                strCmdText = "/K cd " + savedirectory + " & cd  & " + currentscriptname + " &  timeout 10 & exit";

                                System.Diagnostics.Process.Start("CMD.exe", strCmdText);

                                /*
                                Winforms.FolderBrowserDialog FolderDialog = new Winforms.FolderBrowserDialog();
                                FolderDialog.ShowNewFolderButton = false;
                                FolderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
                                Winforms.DialogResult result = FolderDialog.ShowDialog();


                                if (result == Winforms.DialogResult.OK)
                                {
                                    string fpath = FolderDialog.SelectedPath;
                                    scriptpath = fpath;
                                    DirectoryInfo folder = new DirectoryInfo(fpath);


                                    if (folder.Exists)
                                    {

                                        string chars = "2346789ABCDEFGHJKLMNPQRTUVWXYZabcdefghjkmnpqrtuvwxyz";

                                        // create random generator
                                        Random rnd = new Random();
                                        string name;

                                        name = string.Empty;
                                        while (name.Length < 5)
                                        {
                                            name += chars.Substring(rnd.Next(chars.Length), 1);
                                        }

                                        // add extension
                                        name += ".cmd";
                                        currentscriptname = "demoscript-" +name +".cmd";

                                        // check against files in the folder
                                        File.WriteAllText(fpath + "\\demoscript-" + name, roiscript);
                                        System.IO.StreamWriter writer = new System.IO.StreamWriter(fpath + "\\" + path);
                                        writer.Write(text2write);
                                        writer.Close();

                                        MessageBox.Show("Running demoscript-"+name);

                                        //runscript previous generated script in command prompt

                                        string strCmdText;
                                        strCmdText = "/K cd " + scriptpath + " & cd  & "+ currentscriptname +" &  Echo Running Script.........  & timeout 10 & exit";
                                        System.Diagnostics.Process.Start("CMD.exe", strCmdText);


                                    }


                                }*/
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }


            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void gridcamera_MouseMove(object sender, MouseEventArgs e)
        {
            refremask();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            searchfiltertext.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            searchfiltertext.Visibility = Visibility.Collapsed;
        }

        private void applyfiltersetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt.Stop();
                getvalueafterfilter();
                dothisafterfilter();

                dt2.Interval = TimeSpan.FromSeconds(900);
                dt2.Tick += Dt2_Tick;
                
                dt2.Start();

                Debug.WriteLine(datesearchfilter.Text.Replace('/', '-') + " " + timerangefrom.Text + " " + timerangeto.Text + " " + cbbanalyticfiltersearch.Text);

                datedatafilterdashboardtext.Text = datesearchfilter.Text;
                analyticidtextdashboard.Text = cbbanalyticfiltersearch.Text;


            }

            catch(Exception ex)
            {
                MessageBox.Show("Fill in all field.");
            }
            
        }

        private void getvalueforgraphing()
        {
            try
            {
                lastupdateinfotext.Text = DateTime.Now.ToString();
                dashboardsignalstatus.Foreground = new SolidColorBrush(Colors.Green);
                Connectionstatus.Text = "Data Connected";

                getvaluetimespeedstats();
                getvaluegapstats();
                getvaluespeedstats();

                getvaluetotalvehiclemin();
                getvaluetotalvehicleavg();
                getvaluetotalvehiclemax();

                getvaluespeedvehiclemax();
                getvaluespeedvehicleavg();
                getvaluespeedvehiclemin();

                getvaluegapvehicleavg();
                getvaluegapvehiclemax();
                getvaluegapvehiclemin();

                for(int i = 0; i<6; i++)
                {
                    getvaluevehicledailydis(i);
                }
                


            }
            catch (Exception ex)
            {
                dashboardsignalstatus.Foreground = new SolidColorBrush(Colors.Red);
                Connectionstatus.Text = "Data Not Connected";
                refreshcart.IsEnabled = false;
                pause.IsEnabled = false;
                autoref.IsEnabled = false;
                configparameterenabled.Visibility = Visibility.Collapsed;

                dt.Stop();
                packicondbconnectionstatus.Kind = PackIconKind.DatabaseRemove;
                packicondbconnectionstatus.Foreground = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Connection to Database lost! Graph will not update");
            }
        }

        private void getvalueafterfilter()
        {
            try
            {
                getvaluespeedstats(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluegapstats(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluetimespeedstats(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);

                getvaluetotalvehiclemin(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluetotalvehicleavg(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluetotalvehiclemax(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);

                getvaluespeedvehiclemax(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluespeedvehiclemin(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluespeedvehicleavg(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);

                getvaluegapvehicleavg(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluegapvehiclemin(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);
                getvaluegapvehiclemax(datesearchfilter.Text, Int32.Parse(cbbanalyticfiltersearch.Text), timerangefrom.Text, timerangeto.Text);


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #region  //total vehicle min avg max for chart
        private void getvaluetotalvehiclemin()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(total_vehicle) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();
                    
                }
                
            }

            Debug.WriteLine(ListOfColumns);

            Int32.TryParse(ListOfColumns, out totalvehiclemin);

            connection.Close();

        }

        private void getvaluetotalvehiclemin(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(total_vehicle) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + " ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Int32.TryParse(ListOfColumns, out totalvehiclemin);

            connection.Close();

        }

        private void getvaluetotalvehicleavg()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(total_vehicle) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();
                    
                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out totalvehicleavg);

            connection.Close();

        }

        private void getvaluetotalvehicleavg(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(total_vehicle) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out totalvehicleavg);

            connection.Close();

        }

        private void getvaluetotalvehiclemax()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(total_vehicle) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Int32.TryParse(ListOfColumns, out totalvehiclemax);

            connection.Close();

        }

        private void getvaluetotalvehiclemax(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(total_vehicle) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Int32.TryParse(ListOfColumns, out totalvehiclemax);

            connection.Close();

        }
        #endregion
        //end region for total vehicle min max avg


        #region //speed vehicle min avg max for chart


        private void getvaluespeedvehiclemax()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(avg_vehicle_speed) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedmax);

            connection.Close();

        }

        private void getvaluespeedvehiclemax(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(avg_vehicle_speed) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedmax);

            connection.Close();

        }

        private void getvaluespeedvehiclemin()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(avg_vehicle_speed) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedmin);

            connection.Close();

        }

        private void getvaluespeedvehiclemin(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(avg_vehicle_speed) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedmin);

            connection.Close();

        }

        private void getvaluespeedvehicleavg()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(avg_vehicle_speed) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedavg);

            connection.Close();

        }

        private void getvaluespeedvehicleavg(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(avg_vehicle_speed) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out speedavg);

            connection.Close();

        }

        #endregion
        //end region for speed min avg max


        #region //gap vehicle min avg max for chart

        private void getvaluegapvehiclemin()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(avg_vehicle_gap) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapmin);

            connection.Close();

        }

        private void getvaluegapvehiclemin(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MIN(avg_vehicle_gap) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapmin);

            connection.Close();

        }

        private void getvaluegapvehicleavg()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(avg_vehicle_gap) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapavg);

            connection.Close();

        }

        private void getvaluegapvehicleavg(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT AVG(avg_vehicle_gap) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapavg);

            connection.Close();

        }

        private void getvaluegapvehiclemax()
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(avg_vehicle_gap) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapmax);

            connection.Close();

        }

        private void getvaluegapvehiclemax(string date, int analyticid, string timefrom, string timeto)
        {
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT MAX(avg_vehicle_gap) FROM statistics WHERE TIME(time) BETWEEN TIME('" + timefrom + "') AND TIME('" + timeto + "') and date= '" + date + "' and analytic_id =" + analyticid + ";", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(ListOfColumns);

            Double.TryParse(ListOfColumns, out gapmax);

            connection.Close();

        }


        #endregion
        //end region for gap vehicle min avg max for chart


        #region //region for daily distribution

        private void getvaluevehicledailydis(int index)
        {

            int vehiclevalue = index + 1;
            string vehicleno = vehiclevalue.ToString();
            string myConnectionString = "server=" + Dbipaddresstestconn.Text + ";database=" + Dbnametestconn.Text + ";uid=" + Dbusernametestconn.Text + ";pwd=" + Dbpasswordtestconn.Password;
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = new MySqlCommand("SELECT SUM(`vehi_"+vehicleno+"`) FROM statistics WHERE analytic_id = 1 and date= '" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + "' ;", connection);
            connection.Open();
            MySqlDataReader datareader = command.ExecuteReader();
            int ColumnCount = datareader.FieldCount;
            string ListOfColumns = string.Empty;
            while (datareader.Read())
            {
                for (int i = 0; i <= ColumnCount - 1; i++)
                {
                    ListOfColumns = ListOfColumns + datareader[i].ToString();

                }

            }

            Debug.WriteLine(index);
            Int32.TryParse(ListOfColumns, out vehicledistributionarray[index]);

            Debug.WriteLine(vehicledistributionarray[index]);

            connection.Close();
        }

        #endregion

        //end region for daily distribution


    }

}

