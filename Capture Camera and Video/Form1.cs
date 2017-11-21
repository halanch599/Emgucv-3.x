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
using System.Threading;

namespace CaptureCam
{
    public partial class Form1 : Form
    {
        Capture capture;

        public Form1()
        {
            InitializeComponent();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture==null)
            {
                capture = new Emgu.CV.Capture(0);
            }
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                pictureBox1.Image = m.ToImage<Bgr, byte>().Bitmap;
                Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture!=null)
            {
                capture.Pause();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture!=null)
            {
                capture.Dispose();
            }
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture==null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    capture = new Emgu.CV.Capture(ofd.FileName);
                }
            }

            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
        }

        private void pauseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture!=null)
            {
                capture.Pause();
            }
        }

        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (capture!=null)
            {
                capture = null;
            }
        }
    }
}
