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
    public partial class FormContoursSort : Form
    {
        Image<Bgr, byte> imgInput;
        private Bitmap img;
        bool show = false;

        public FormContoursSort()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imgInput = new Image<Bgr, byte>(ofd.FileName);
                pictureBox1.Image = imgInput.Bitmap;
            }
        }

        private async void findContoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> imgout = imgInput.Convert<Gray, byte>().Not().ThresholdBinary(new Gray(50), new Gray(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hier = new Mat();

            CvInvoke.FindContours(imgout, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            if (contours.Size>0)
            {
                show = true;

                for (int i = 0; i < 10; i++)
                {
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                    imgInput.ROI = rect;
                   img =imgInput.Copy().Bitmap;
                    this.Invalidate();
                    await Task.Delay(500);
                }
                show = false;
            }
            return;
            Dictionary<int, double> dict = new Dictionary<int, double>();

            if (contours.Size>0)
            {
                for (int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                    if (rect.Width>50 && rect.Height>30 && area<3000)
                    {
                        dict.Add(i, area);
                    }
                }
            }



            var item = dict.OrderByDescending(v => v.Value);

            Image<Bgr, byte> imgout1 = new Image<Bgr, byte>(imgInput.Width, imgInput.Height, new Bgr(0, 0, 0));

            foreach (var it in item)
            {
                int key = int.Parse(it.Key.ToString());
                Rectangle rect = CvInvoke.BoundingRectangle(contours[key]);
                //CvInvoke.DrawContours(imgInput, contours, key, new MCvScalar(255, 255, 255),4);
                CvInvoke.Rectangle(imgout1, rect, new MCvScalar(255, 255, 255), 3);
            }

            pictureBox2.Image = imgout1.Bitmap;

        }

        private void FormContoursSort_Paint(object sender, PaintEventArgs e)
        {
            if (show==true)
            {
                pictureBox2.Image = img;

            }
        }

        private void FormContoursSort_Load(object sender, EventArgs e)
        {

        }
    }
}
