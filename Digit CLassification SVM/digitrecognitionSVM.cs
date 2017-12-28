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
using Emgu.CV.ML;
using System.IO;

namespace Emgucv33Apps
{
    public partial class digitrecognitionSVM : Form
    {
        string TraingDataPath = @"D:\Digits\mnist_train.csv";
        string TestDataPath = @"D:\Digits\mnist_test.csv";

        Matrix<float> TrainData;
        Matrix<float> TestData;
        Matrix<int> TrainLabel;
        Matrix<int> TestLabel;

        SVM svm;
        int Counter = 0;
        bool IsDisplayImage = false;

        public digitrecognitionSVM()
        {
            InitializeComponent();
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadTrainData();
                LoadTestData();
                MessageBox.Show("Data loaded.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadTrainData()
        {
            List<float[]> trainList = new List<float[]>();
            List<int> trainLabel = new List<int>();

            StreamReader reader = new StreamReader(TraingDataPath);

            string line = "";
            if (!File.Exists(TraingDataPath))
            {
                throw new Exception("File Not found");
            }

            while ((line =  reader.ReadLine())!=null)
            {
                int firstIndex = line.IndexOf(',');
                int currentLabel =  Convert.ToInt32( line.Substring(0, firstIndex));
                string currentData = line.Substring(firstIndex + 1);
                float[] data = currentData.Split(',').Select(x => float.Parse(x)).ToArray();

                trainList.Add(data);
                trainLabel.Add(currentLabel);

            }

            TrainData = new Matrix<float>(To2D<float>(trainList.ToArray()));
            TrainLabel = new Matrix<int>(trainLabel.ToArray());

        }

        private void LoadTestData()
        {
            List<float[]> trainList = new List<float[]>();
            List<int> trainLabel = new List<int>();

            StreamReader reader = new StreamReader(TestDataPath);

            string line = "";
            if (!File.Exists(TraingDataPath))
            {
                throw new Exception("File Not found");
            }

            while ((line = reader.ReadLine()) != null)
            {
                int firstIndex = line.IndexOf(',');
                int currentLabel = Convert.ToInt32(line.Substring(0, firstIndex));
                string currentData = line.Substring(firstIndex + 1);
                float[] data = currentData.Split(',').Select(x => float.Parse(x)).ToArray();

                trainList.Add(data);
                trainLabel.Add(currentLabel);

            }

            TestData = new Matrix<float>(To2D<float>(trainList.ToArray()));
            TestLabel = new Matrix<int>(trainLabel.ToArray());

        }

        // reference https://stackoverflow.com/questions/26291609/converting-jagged-array-to-2d-array-c-sharp
        private T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TrainData==null)
            {
                return;
            }

            if (Counter>=0)
            {

                Image<Gray, byte> imgout = TrainData.GetRow(Counter).Mat.Reshape(0,28).ToImage<Gray, byte>().ThresholdBinary(new Gray(30), new Gray(255));
                pictureBox1.Image = imgout.Bitmap;
                Counter--;

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (TrainData == null)
            {
                return;
            }

            if (Counter < TrainData.Rows-1)
            {
                Counter++;

                Image<Gray, byte> imgout = TrainData.GetRow(Counter).Mat.Reshape(0, 28).ToImage<Gray, byte>().ThresholdBinary(new Gray(30), new Gray(255));
                pictureBox1.Image = imgout.Bitmap;
            }
        }

        private void trainSVMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("svm.txt"))
                {
                    svm = new SVM();
                    FileStorage file = new FileStorage("svm.txt", FileStorage.Mode.Read);
                    svm.Read(file.GetNode("opencv_ml_svm"));
                }
                else
                {
                    svm = new SVM();
                    svm.C = 100;
                    svm.Type = SVM.SvmType.CSvc;
                    svm.Gamma = 0.005;
                    svm.SetKernel(SVM.SvmKernelType.Linear);
                    svm.TermCriteria = new MCvTermCriteria(1000, 1e-6);
                    svm.Train(TrainData, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, TrainLabel);
                    svm.Save("svm.txt");
                }
                MessageBox.Show("SVM is trained.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void testSVMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TestData==null)
            {
                return;
            }

            if (svm==null)
            {
                return;
            }
            try
            {
                int counter = 0;
                for (int i = 0; i < TestData.Rows; i++)
                {
                    Matrix<float> row = TestData.GetRow(i);
                    float predict = svm.Predict(row);
                    lblTest.Text = "Input Label:" + TestLabel[i, 0].ToString();
                    lblOouputLabel.Text = "Predicted Label" + predict.ToString();
                    if (predict==TestLabel[i,0])
                    {
                        counter += 1;
                    }

                    if (IsDisplayImage==true)
                    {
                        Image<Gray, byte> imgout = TestData.GetRow(Counter).Mat.Reshape(0, 28).ToImage<Gray, byte>().ThresholdBinary(new Gray(30), new Gray(255));
                        pictureBox1.Image = imgout.Bitmap;
                        await Task.Delay(1000);
                    }
                    else
                    {
                        await Task.Delay(1);
                    }
                }

                lblAccuracy.Text = "Accuracy = " + (counter / (float)(TestData.Rows));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbShowData_CheckedChanged(object sender, EventArgs e)
        {
            IsDisplayImage = cbShowData.Checked;
        }
    }
}
