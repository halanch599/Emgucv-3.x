using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgucv33Apps
{
    public partial class FormWriteVideo : Form
    {
        double TotalFrame;
        double Fps;
        int FrameNo;
        VideoCapture capture;

        public FormWriteVideo()
        {
            InitializeComponent();
        }

        private void readVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.Bitmap;

                TotalFrame = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            }
        }

        private void writeVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture==null)
            {
                return;
            }

            int fourcc = Convert.ToInt32( capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FourCC));
            int Width = Convert.ToInt32( capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth));
            int Height= Convert.ToInt32(capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight));


            string destionpath = @"E:\PROJECTS\Development Projects\Tutorials\Emgucv\EmguCV # 21 Write video in Emgu CV\output.mp4";
            VideoWriter writer = new VideoWriter(destionpath, fourcc, Fps, new Size(Width, Height), true);
            Image<Bgr, byte> logo = new Image<Bgr, byte>(@"E:\PROJECTS\Development Projects\Tutorials\Emgucv\EmguCV # 21 Write video in Emgu CV\logo.jpg");
            Mat m = new Mat();

            while (FrameNo<500)
            {

                capture.Read(m);
                Image<Bgr, byte> img = m.ToImage<Bgr, byte>();
                img.ROI = new Rectangle(Width - logo.Width - 30, 10,logo.Width,logo.Height);
                logo.CopyTo(img);

                img.ROI = Rectangle.Empty;

                writer.Write(img.Mat);
                FrameNo++;
            }
            if (writer.IsOpened)
            {
                writer.Dispose();
            }

            MessageBox.Show("Completed.");
        }
    }
}
