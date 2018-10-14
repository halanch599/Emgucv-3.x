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
    public partial class FormCCStats : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Gray, byte> imgCC;
        CCStatsOp[] statsop;
        MCvPoint2D64f[] centroidPoints;


        public struct CCStatsOp
        {
            public Rectangle Rectangle;
            public int Area;
        }


        public FormCCStats()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                imgInput = new Image<Bgr, byte>(dialog.FileName);
                pictureBox1.Image = imgInput.Bitmap;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (imgInput==null)
            {
                return;
            }


            try
            {
                var temp = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255))
                    .Dilate(1).Erode(1);

                Mat imgLabel = new Mat();
                Mat stats = new Mat();
                Mat centroids = new Mat();

                var nLabels = CvInvoke.ConnectedComponentsWithStats(temp, imgLabel, stats, centroids);

                imgCC = imgLabel.ToImage<Gray, byte>();

                centroidPoints = new MCvPoint2D64f[nLabels];
                centroids.CopyTo(centroidPoints);

                statsop = new CCStatsOp[nLabels];
                stats.CopyTo(statsop);

                pictureBox2.Image = temp.Bitmap;

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (imgCC==null)
            {
                return;
            }


            try
            {
                int label = (int)imgCC[e.Y, e.X].Intensity;

                if (label!=0)
                {
                    var temp = imgCC.InRange(new Gray(label), new Gray(label));

                    int x = (int)centroidPoints[label].X;
                    int y = (int)centroidPoints[label].Y;

                    var t = imgInput.Copy();
                    CvInvoke.PutText(t, "o", new Point(x, y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 0.8, new MCvScalar(0, 0, 255), 2);

                    Rectangle rect = statsop[label].Rectangle;
                    t.Draw(rect, new Bgr(0, 0, 255), 2);

                    label1.Text = statsop[label].Area.ToString();

                    pictureBox1.Image = t.Bitmap;
                    pictureBox2.Image = temp.Bitmap;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
