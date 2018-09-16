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
    public partial class FormShapeDetection : Form
    {

        Image<Bgr, byte> imgInput;
        public FormShapeDetection()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(dialog.FileName);
                    pictureBox1.Image = imgInput.Bitmap;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void detectShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgInput==null)
            {
                return;
            }

            try
            {
                var temp = imgInput.SmoothGaussian(5).Convert<Gray, byte>().ThresholdBinaryInv(new Gray(230), new Gray(255));

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                Mat m = new Mat();

                CvInvoke.FindContours(temp, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                for (int i = 0; i < contours.Size; i++)
                {
                    double perimeter = CvInvoke.ArcLength(contours[i], true);
                    VectorOfPoint approx = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(contours[i], approx, 0.04 * perimeter, true);

                    CvInvoke.DrawContours(imgInput, contours, i, new MCvScalar(0, 0, 255), 2);

                    //moments  center of the shape

                    var moments = CvInvoke.Moments(contours[i]);
                    int x = (int)(moments.M10 / moments.M00);
                    int y = (int)(moments.M01 / moments.M00);

                    if (approx.Size == 3)
                    {
                        CvInvoke.PutText(imgInput, "Triangle", new Point(x, y),
                            Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                    }

                    if (approx.Size == 4)
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);

                        double ar = (double)rect.Width / rect.Height;

                        if (ar >= 0.95 && ar<=1.05)
                        {
                            CvInvoke.PutText(imgInput, "Square", new Point(x, y),
                            Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }
                        else
                        {
                            CvInvoke.PutText(imgInput, "Rectangle", new Point(x, y),
                            Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                        }
                        
                    }

                    if (approx.Size == 6)
                    {
                        CvInvoke.PutText(imgInput, "Hexagon", new Point(x, y),
                            Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                    }


                    if (approx.Size > 6)
                    {
                        CvInvoke.PutText(imgInput, "Circle", new Point(x, y),
                            Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
                    }

                    pictureBox2.Image = imgInput.Bitmap;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
