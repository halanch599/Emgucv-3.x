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
using Emgu.CV.Cuda;

namespace tutorial2
{
    public partial class FormMeanshift : Form
    {
        Image<Bgra, byte> ImgInput;
        public FormMeanshift()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImgInput = new Image<Bgra, byte>(ofd.FileName);
                pictureBox1.Image = ImgInput.Bitmap;
            }
        }

        private void meanshiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CalcuateMeanshift(ImgInput);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Test");
            }
        }
        public void CalcuateMeanshift(Image<Bgra, byte> imgInput, int spatialWindow = 5, int colorWindow = 5, int MinSegmentSize = 20)
        {

            if (imgInput == null)
            {
                return;
            }
            try
            {
                Image<Bgra, byte> imgOutput = new Image<Bgra, byte>(imgInput.Width, imgInput.Height, new Bgra(0, 0, 0, 0));

                //convert the image to BGRA as it requires a BGRA to pass it in constructor of CudaImage
                CudaImage<Bgra, byte> _inputCuda = new CudaImage<Bgra, byte>(imgInput);
                CudaInvoke.MeanShiftSegmentation(_inputCuda, imgOutput, spatialWindow, colorWindow, MinSegmentSize, new MCvTermCriteria(1, .001));
                pictureBox2.Image = imgOutput.Bitmap;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Meam shift error: " + ex.Message);
            }
        }

    }
}
