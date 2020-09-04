using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace systemapps
{
    /// <summary>
    /// Interaction logic for videoscreenshot.xaml
    /// </summary>
    public partial class videoscreenshot : Window
    {
        public videoscreenshot()
        {
            InitializeComponent();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            medEl.Stop();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            medEl.Play();
        }

        private void capture_Click(object sender, RoutedEventArgs e)
        {
            byte[] screenshot = medEl.GetScreenShot(1, 100);
            FileStream fileStream = new FileStream(@"Capture.jpg", FileMode.Create, FileAccess.ReadWrite);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Write(screenshot);
            binaryWriter.Close();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            medEl.Pause();
        }
    }
}

public static class screenshot
{
    public static byte[] GetScreenShot(this UIElement source, double scale, int quality)
    {
        double actualheight = source.RenderSize.Height;
        double actualwidth = source.RenderSize.Width;
        double renderheight = actualheight * scale;
        double renderwidth = actualwidth * scale;


        RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)renderwidth, (int)renderheight, 96, 96, PixelFormats.Pbgra32);
        VisualBrush sourcebrush = new VisualBrush(source);

        DrawingVisual drawingvisual = new DrawingVisual();
        DrawingContext drawingContext = drawingvisual.RenderOpen();

        using (drawingContext)
        {
            drawingContext.PushTransform(new ScaleTransform(scale, scale));
            drawingContext.DrawRectangle(sourcebrush, null, new Rect(new Point(0, 0), new Point(actualwidth, actualheight)));
        }
        renderTarget.Render(drawingvisual);

        JpegBitmapEncoder jpgencoder = new JpegBitmapEncoder();
        jpgencoder.QualityLevel = quality;
        jpgencoder.Frames.Add(BitmapFrame.Create(renderTarget));

        Byte[] imagearray;

        using (MemoryStream outputStream = new MemoryStream())
        {
            jpgencoder.Save(outputStream);
            imagearray = outputStream.ToArray();
        }
        return imagearray;
    }
}