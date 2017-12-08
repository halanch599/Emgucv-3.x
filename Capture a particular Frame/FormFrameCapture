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
    public partial class FormFrameCapture : Form
    {
        double TotalFrame;
        double Fps;
        int FrameNo;
        bool IsReadingFrame;

        VideoCapture capture;

        public FormFrameCapture()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.Bitmap;

                TotalFrame = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            }

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (capture==null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();


        }

        private async void ReadAllFrames()
        {

            Mat m = new Mat();
            while (IsReadingFrame ==true && FrameNo < TotalFrame)
            {
                FrameNo += Convert.ToInt16( numericUpDown1.Value);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameNo);
                capture.Read(m);
                pictureBox1.Image = m.Bitmap;
                await Task.Delay(1000 / Convert.ToInt16( Fps));
                label1.Text = FrameNo.ToString() + "/" + TotalFrame.ToString();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            IsReadingFrame = false;
        }
    }
}
