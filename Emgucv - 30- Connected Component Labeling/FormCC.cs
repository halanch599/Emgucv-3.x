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
    public partial class FormCC : Form
    {

        Image<Bgr, byte> imgInput;
        Image<Gray, byte> CC;

        public FormCC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if(dialog.ShowDialog()==DialogResult.OK)
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
                var temp = imgInput.Convert<Gray, byte>().ThresholdBinary(new Gray(100),
                    new Gray(255)).Dilate(1).Erode(1);


                Mat imgLabel = new Mat();
                int nLabel =  CvInvoke.ConnectedComponents(temp, imgLabel);

                CC = imgLabel.ToImage<Gray, byte>();
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
               int label =  (int) CC[e.Y, e.X].Intensity;

                if (label!=0)
                {
                    var temp = CC.InRange(new Gray(label), new Gray(label));
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
