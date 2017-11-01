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

namespace imageOverlay
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Gray, byte> imgOutput;

        public Form1()
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

        private void rangeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formParameters fp = new formParameters(this);
            fp.Show();
        }

        public void ApplyRangeFitler(int min, int max)
        {
            try
            {
                imgOutput = imgInput.Convert<Gray, byte>().InRange(new Gray(min), new Gray(max)).Canny(10, 50);

                pictureBox2.Image = imgOutput.Bitmap;
                pictureBox2.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
        private void overlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput==null)
                {
                    return;
                }
                Image<Bgr, byte> temp = imgInput.Clone();
                temp.SetValue(new Bgr(0,0,255), imgOutput);
                pictureBox2.Image = temp.Bitmap;

            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
