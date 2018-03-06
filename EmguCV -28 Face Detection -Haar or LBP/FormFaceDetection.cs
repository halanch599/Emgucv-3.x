using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgucv33Apps
{
    public partial class FormFaceDetection : Form
    {
        Image<Bgr, byte> imgInput;
        public FormFaceDetection()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(dialog.FileName);
                    pictureBox1.Image = imgInput.Bitmap;
                }
                else
                {
                    throw new Exception("No file selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void detectFaceHaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput==null)
                {
                    throw new Exception("Please select and image");
                }

                DetectFaceHaar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DetectFaceHaar()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../data/haarcascade_frontalface_default.xml");
                string eyePath = Path.GetFullPath(@"../../data/haarcascade_eye.xml");


                CascadeClassifier classifierFace = new CascadeClassifier(facePath);
                CascadeClassifier classifierEye = new CascadeClassifier(eyePath);

                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 4);
                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(0, 0, 255), 2);

                    imgGray.ROI = face;
                 Rectangle[]eyes=   classifierEye.DetectMultiScale(imgGray, 1.1, 4);
                    foreach (var eye in eyes)
                    {
                        var e = eye;
                        e.X += face.X;
                        e.Y += face.Y;
                        imgInput.Draw(e, new Bgr(0, 255, 0), 2);
                    }
                }
                pictureBox1.Image = imgInput.Bitmap;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DetectFaceLBP()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../data/lbpcascade_frontalface.xml");
                CascadeClassifier classifierFace = new CascadeClassifier(facePath);

                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 4);
                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(255, 0, 0), 2);
                }
                pictureBox1.Image = imgInput.Bitmap;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void detectFaceLBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput==null)
                {
                    throw new Exception("No image selected");
                }

                DetectFaceLBP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
