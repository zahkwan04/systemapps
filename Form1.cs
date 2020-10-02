using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace systemapps
{
    public partial class Form1 : MaterialForm
    {

        string readimgpath;
        Bitmap newimage = null;
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
        int[] roitype = new int[6]; //1= positive, 2 = normal negative, 3= overlap -ve roi, 4=overlap - special roi
        public Form1()
        {

            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            //string readimgpath = File.ReadAllText(@"imagepath.txt");
            //this.pictureBox1.Image = Image.FromFile(readimgpath);
            //this.pictureBox1.Load(readimgpath);
            imgautoload();


            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Mouse_ImageClick);
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Mouse_ImageDClick);
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            for (int j = 0; j < roitype.Length; j++)
            {
                roitype[j] = 0;
            }
        }

        void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.Image;
            RedrawPolygon();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            openFileDialog1.InitialDirectory = "'C:\'";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Title = "Open File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string thumb = openFileDialog1.FileName;
                Image oriimage = Image.FromFile(thumb);
                newimage = new Bitmap(oriimage, 352, 288);


                if (oriimage.Width <= 800 && oriimage.Height <= 600)
                {
                    // newimage = new Bitmap(oriimage, oriimage.Width, oriimage.Height);
                    //  Graphics g = pictureBox1.CreateGraphics();
                    //  g.Clear(Color.AliceBlue);
                    // pictureBox1.Image = newimage;
                    oriWidth = oriimage.Width;
                    oriHeight = oriimage.Height;


                }

                else
                {
                    int newWidth = oriimage.Width / 2;
                    int newHeight = oriimage.Height / 2;
                    // Bitmap newnewimage = new Bitmap(oriimage, newWidth, newHeight);
                    // Graphics g = pictureBox1.CreateGraphics();
                    // g.Clear(Color.AliceBlue);
                    //pictureBox1.Image = newnewimage;

                    refWidth = newWidth;
                    refHeight = newHeight;
                }


                // pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                Graphics g = pictureBox1.CreateGraphics();
                g.Clear(Color.AliceBlue);
                pictureBox1.Image = newimage;//Image.FromFile(thumb);
            }
        }



        private void Mouse_ImageClick(object sender, MouseEventArgs e)
        {
            if (noofroidrawn < 6 && (falseroi == true || specialroi == true))
            {
                // Bitmap roiimage = new Bitmap(pictureBox1.Width, pictureBox1.Height, g);

                Object testimage = pictureBox1.Image.Clone();
                Bitmap testbitmap = (Bitmap)testimage;
                Color pixelcolor = testbitmap.GetPixel(e.X, e.Y);
                Point pointclick = new Point(e.X, e.Y);
                Boolean inpoly = false;
                Boolean isitpostv = false;
                int noroi = 0;

                foreach (Point[] polyplot in drawnroi)
                {
                    noroi++;
                    if (polyplot != null)
                    {
                        inpoly = PointInPolygon(pointclick, polyplot);
                        if (roitype[noroi] == 1)
                        {
                            isitpostv = true;
                        }
                        if (inpoly == true)
                            break;
                    }
                }

                if (overlaproi == 1 && inpoly == true)
                {
                    overlaproi = 3; //2=no overlap before, 3=got overlap before
                    if (specialroi == true && isitpostv == false)
                    {
                        MessageBox.Show("Special ROI must be drawn inside a positive roi");
                        overlaproi = 1;
                        return;
                    }

                }
                else if (overlaproi == 1 && inpoly == false)
                {
                    overlaproi = 2; //2=no overlap before, 3=got overlap before
                    if (specialroi == true)
                    {
                        MessageBox.Show("Special ROI must be drawn inside a positive roi");
                        overlaproi = 1;
                        return;
                    }
                }
                else if (overlaproi == 3 && inpoly == false)
                {
                    MessageBox.Show("Partial Overlap is not allowed, please draw with no overlap or fully overlap");
                    return;
                }
                else if (overlaproi == 2 && inpoly == true)
                {
                    MessageBox.Show("Partial Overlap is not allowed, please draw with no overlap or fully overlap");
                    return;
                }

                GC.Collect();

            }

            if ((noofroidrawn < 20 && (positiveroi < 10 || falseroi == true || specialroi == true)) && pictureBox1.Image != null) //how if change max roi to 6..or need to separate for +ve n -ve
            {
                if (clickcount == 0)
                    polycoorobj = new Point[20];
                //fdrawing = 1;

                Point point1 = new Point(e.X, e.Y);

                polycoorobj[clickcount] = point1;
                clickcount++;

                //foreach (Point drawdot in polycoorobj)
                Graphics g = pictureBox1.CreateGraphics();
                if (point1.Y != 0 && point1.X != 0)
                    g.FillEllipse(Brushes.Aquamarine, new Rectangle(point1.X, point1.Y, 7, 7));
                //}

                if (clickcount == 15)
                {
                    Mouse_ImageDClick(null, null);
                    clickcount = 0; //PR (Issue8) 20th May 2010
                }
            }
        }

        private void Mouse_ImageDClick(object sender, MouseEventArgs e)
        {
            if (noofroidrawn < 20 && (positiveroi < 10 || falseroi == true || specialroi == true)) //check for no of roi drawn, can change to 6?
            {

                if (clickcount > 2)
                {
                    Color myColor;

                    myColor = Color.FromArgb(127, 23, 56, 78);

                    SolidBrush mybrush = new SolidBrush(myColor);
                    Graphics g = pictureBox1.CreateGraphics();
                    Point[] localcoor = new Point[clickcount];
                    for (int i = 0; i < clickcount; i++)
                    {
                        localcoor[i] = polycoorobj[i];
                    }

                    if (localcoor != null)
                    {
                        if (localcoor != null)
                        {
                            g.DrawPolygon(new Pen(Color.Red), localcoor);
                            g.FillPolygon(mybrush, localcoor);
                            SaveROIInfo(localcoor);
                            noofroidrawn++;
                            if (polycoorobj != null)
                                Array.Clear(polycoorobj, 0, polycoorobj.Length);
                            for (int k = 0; k < roitype.Length; k++)
                            {
                                if (roitype[k] == 0)
                                {
                                    if (overlaproi == 1 && falseroi == false)
                                    {
                                        roitype[k] = 1; //positive roi
                                        positiveroi++;
                                        break;
                                    }
                                    else if (overlaproi == 2 && falseroi == true)
                                    {
                                        roitype[k] = 2; //-ve ROI with no overlap
                                        break;
                                    }
                                    else if (overlaproi == 3 && falseroi == false && specialroi == true)
                                    {
                                        roitype[k] = 4; //-ve ROI with overlap but special ROI
                                        break;
                                    }
                                    else if (overlaproi == 3 && falseroi == true)
                                    {
                                        roitype[k] = 3; //-ve ROI with overlap
                                        break;
                                    }
                                }
                            }
                            clickcount = 0;
                        }
                    }
                }
            }
            overlaproi = 1;
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);                          
        //}

        private void RedrawPolygon()
        {
            if (noofroidrawn > 0)
            {
                Color myColor;
                myColor = Color.FromArgb(127, 23, 56, 78);
                // pictureBox1.Image = pictureBox1.Image;
                //pictureBox1.Image.Dispose();
                // pictureBox1.Image = (Image)newimage;//Image.FromFile(thumb);
                //pictureBox1.Invalidate();
                Graphics t = pictureBox1.CreateGraphics();
                SolidBrush mybrush = new SolidBrush(myColor);
                Bitmap newnewimage = new Bitmap(newimage);//, t);

                Graphics abc;
                abc = Graphics.FromImage(newnewimage);

                foreach (Point[] polyplot in drawnroi)
                {
                    if (polyplot != null)
                    {
                        abc.DrawPolygon(new Pen(Color.Red), polyplot);
                        abc.FillPolygon(mybrush, polyplot);

                        //m++;
                    }

                }
                GC.Collect();
                abc.Dispose();
                pictureBox1.Image = newnewimage;

            }
            else
            {
                pictureBox1.Image = newimage;//Image.FromFile(thumb);
            }
        }

        private void RedrawPolygon2()
        {
            if (noofroidrawn > 0)
            {
                Color myColor;
                myColor = Color.FromArgb(127, 23, 56, 78);
                Graphics g = pictureBox1.CreateGraphics();
                SolidBrush mybrush = new SolidBrush(myColor);
                foreach (Point[] polyplot in drawnroi)
                {
                    if (polyplot != null)
                    {
                        g.DrawPolygon(new Pen(Color.Red), polyplot);
                        g.FillPolygon(mybrush, polyplot);
                        //m++;
                    }
                }
            }
            else
            {
                pictureBox1.Image = newimage;//Image.FromFile(thumb);
            }
        }

        private void SaveROIInfo(Point[] savecoor)
        {
            drawnroi[noofroidrawn] = new Point[savecoor.Length];
            for (int j = 0; j < savecoor.Length; j++)
            {
                drawnroi[noofroidrawn][j] = savecoor[j];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Graphics g = pictureBox1.CreateGraphics();
            pictureBox1.Image = pictureBox1.Image;
            if (polycoorobj != null)
                Array.Clear(polycoorobj, 0, polycoorobj.Length);
            if (drawnroi != null)
                Array.Clear(drawnroi, 0, drawnroi.Length);
            noofroidrawn = 0;
            positiveroi = 0;
            clickcount = 0;
            specialroi = false;
            falseroi = false;

        }



        private void DrawROIMap()
        {
            string roipoints = "ROI Points";

            // int m = 0;
            // int k = 6; // 6 = special roi, 7 = not overlapping roi, 8 = overlapping roi
            string filename = null;
            string roifile = roipoints + ".bmp";
            if ((File.Exists(Application.StartupPath + "//" + roifile)))
            {
                File.Delete(Application.StartupPath + "//" + roifile);
                //break;
            }
            filename = roifile;


            if (filename != null)
            {
                Color myColor = Color.FromArgb(255, 255, 255);
                SolidBrush mybrush = new SolidBrush(myColor);
                Graphics g = pictureBox1.CreateGraphics();

                Bitmap roiimage = new Bitmap(pictureBox1.Width, pictureBox1.Height, g);
                Graphics goff;
                goff = Graphics.FromImage(roiimage);

                goff.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), 0, 0, roiimage.Width, roiimage.Height);


                GC.Collect();


                roiimage.Save(Application.StartupPath + "//" + filename, System.Drawing.Imaging.ImageFormat.Bmp);
                goff.Dispose();

                GC.Collect();



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newtextroi;
            int x1init = 0;
            int y1init = 0;

            if (pictureBox1.Image != null)


            {
                DrawROIMap();
                //1
                string filename = null;
                string roifile = "ROI Points" + ".txt";

                filename = roifile;
                string readimgpath = File.ReadAllText(@"imagepath.txt");
                //string thumb = openFileDialog1.FileName;
                string thumb = readimgpath;

                Image oriimage = Image.FromFile(thumb);
                StreamWriter sw = new StreamWriter(filename);

                //refWidth = pictureBox1.Width;
                // refHeight = pictureBox1.Height;

                if (oriimage.Width <= 800 && oriimage.Height <= 600)
                {

                    string orisize;
                    orisize = string.Concat(oriWidth.ToString(), ",", oriHeight.ToString(), ";");
                    sw.Write(orisize);
                }

                else
                {
                    string mytext;
                    //mytext = string.Concat(refWidth.ToString(), ",", refHeight.ToString(), ";");
                    mytext = string.Concat(oriimage.Width.ToString(), ",", oriimage.Height.ToString(), ";");
                    sw.Write(mytext);
                }

                //3
                foreach (Point[] polyplot in drawnroi)
                {
                    string test = "";
                    string initialpoint = "";


                    if (polyplot != null)
                    {
                        foreach (Point A in polyplot)
                        {
                            if (x1init == 0 || y1init == 0)
                            {
                                x1init = A.X;
                                y1init = A.Y;
                            }

                            int x1 = A.X;
                            int y1 = A.Y;

                            //A.X;
                            //A.Y;

                            test = string.Concat(test, x1.ToString(), ",", y1.ToString(), ";");
                            initialpoint = string.Concat(initialpoint, x1init.ToString(), ",", y1init.ToString(), ";");
                        }

                        
                        // Remove last character from a string  
                        initialpoint = initialpoint.Remove(initialpoint.Length - 1, 1);
                        Debug.WriteLine(initialpoint);
                        sw.WriteLine(test + initialpoint);



                    }
                }

                sw.Close();
              


                if (polycoorobj != null)
                    Array.Clear(polycoorobj, 0, polycoorobj.Length);
                if (drawnroi != null)
                    Array.Clear(drawnroi, 0, drawnroi.Length);
                noofroidrawn = 0;
                clickcount = 0;
                MessageBox.Show("Roi Image saved successfully");
                //  File.Encrypt("ROI Points.txt");
                RedrawPolygon();



            }

            this.Hide();

        }




        static bool PointInPolygon(Point p, Point[] poly)
        {
            Point p1, p2;
            bool inside = false;
            if (poly.Length < 3)
            {
                return inside;
            }

            Point oldPoint = new Point(poly[poly.Length - 1].X, poly[poly.Length - 1].Y);

            for (int i = 0; i < poly.Length; i++)
            {
                Point newPoint = new Point(poly[i].X, poly[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X) && ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X)
                < ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }
            return inside;
        }

        private void button8_Click(object sender, EventArgs e) //clear last drawn dot or last drawn roi
        {
            if (clickcount > 0 && polycoorobj != null)
            {
                Array.Clear(polycoorobj, 0, polycoorobj.Length);
                RedrawPolygon();
            }
            else
            {
                if (drawnroi != null && noofroidrawn > 0)
                {
                    Array.Clear(drawnroi, noofroidrawn - 1, 1);
                    noofroidrawn--;
                    RedrawPolygon();

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void imgautoload()
        {

            try
            {
                readimgpath = File.ReadAllText(@"imagepath.txt");

                //string thumb = openFileDialog1.FileName;
                Image oriimage = Image.FromFile(readimgpath);
               // newimage = new Bitmap(oriimage, 352, 288);
                newimage = new Bitmap(oriimage, 361, 295);



                if (oriimage.Width <= 800 && oriimage.Height <= 600)
                {
                    // newimage = new Bitmap(oriimage, oriimage.Width, oriimage.Height);
                    //  Graphics g = pictureBox1.CreateGraphics();
                    //  g.Clear(Color.AliceBlue);
                    // pictureBox1.Image = newimage;
                    oriWidth = oriimage.Width;
                    oriHeight = oriimage.Height;


                }

                else
                {
                    int newWidth = oriimage.Width / 2;
                    int newHeight = oriimage.Height / 2;
                    // Bitmap newnewimage = new Bitmap(oriimage, newWidth, newHeight);
                    // Graphics g = pictureBox1.CreateGraphics();
                    // g.Clear(Color.AliceBlue);
                    //pictureBox1.Image = newnewimage;

                    refWidth = newWidth;
                    refHeight = newHeight;
                }


                // pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                Graphics g = pictureBox1.CreateGraphics();
                g.Clear(Color.AliceBlue);
                pictureBox1.Image = newimage;//Image.FromFile(thumb);
                readimgpath = "";
            }

            catch (Exception ex)
            {
                MessageBox.Show("No valid image was found");
                Debug.WriteLine(ex.Message);
            }

        }
        private void closebutton_click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}


