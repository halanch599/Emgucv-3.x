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
    public partial class FormCropCC : Form
    {

        Image<Bgr, byte> imgInput;
        Image<Gray, byte> CC;

        public FormCropCC()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                imgInput = new Image<Bgr, byte>(dialog.FileName);
                pictureBox1.Image = imgInput.Bitmap;
            }
        }

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgInput==null)
            {
                return;
            }

            try
            {
                var temp = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255))
                                    .Dilate(1).Erode(1);
                Mat labels = new Mat();
                int nLabels = CvInvoke.ConnectedComponents(temp, labels);
                CC = labels.ToImage<Gray, byte>();
                pictureBox2.Image = temp.Bitmap;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (CC==null)
            {
                return;
            }

            try
            {
                int label =(int) CC[e.Y, e.X].Intensity;
                if (label!=0)
                {
                    var temp = CC.InRange(new Gray(label), new Gray(label));
                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    Mat m = new Mat();

                    CvInvoke.FindContours(temp, contours, m, Emgu.CV.CvEnum.RetrType.External, 
                        Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                    if (contours.Size>0)
                    {
                        Rectangle bbox = CvInvoke.BoundingRectangle(contours[0]);

                        imgInput.ROI = bbox;
                        var img = imgInput.Copy();

                        imgInput.ROI = Rectangle.Empty;
                        pictureBox2.Image = img.Bitmap;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
