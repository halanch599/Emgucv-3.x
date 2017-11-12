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
using Emgu.CV.Util;


namespace Emgucv33Apps
{
    public partial class formcharactsegmentation : Form
    {
        Image<Bgr, byte> imgInput;
        Bitmap img;
        bool show = false;

        public formcharactsegmentation()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog()==DialogResult.OK)
            {
                imgInput = new Image<Bgr, byte>(ofd.FileName);
                pictureBox1.Image = imgInput.Bitmap;
            }
        }

        private async void segmentCharachtersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> imgout = imgInput.Convert<Gray, byte>().Not().ThresholdBinary(new Gray(50), new Gray(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hier = new Mat();

            CvInvoke.FindContours(imgout, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            show = true;
            if (contours.Size>0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                    imgInput.ROI = rect;

                    img = imgInput.Copy().Bitmap;
                    imgInput.ROI = Rectangle.Empty;
                    this.Invalidate();

                    await Task.Delay(500);
                }
                show = false;
            }

         }

        private void formcharactsegmentation_Paint(object sender, PaintEventArgs e)
        {
            if (show==true)
            {
                pictureBox2.Image = img;
            }
        }
    }
}
