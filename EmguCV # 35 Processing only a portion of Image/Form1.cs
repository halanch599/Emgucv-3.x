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
using Emgu.CV.UI;
using ZedGraph;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.CvEnum;

namespace SemanticSegmentationNET
{
    
    public partial class Form1 : Form
    {
        //Install-Package Emgu.CV.runtime.windows -Version 4.2.0.3662
        Image<Bgr, byte> imgInput;
        Rectangle rect=Rectangle.Empty;
        Dictionary<string, Image<Bgr, byte>> imgList;
        private bool showPixelValue=false;
        int rows, cols;
        private bool down;
        Point start, end;
        bool selecting = false;
        Rectangle rectROI ;
        bool SelectROI = false;
        Point StartROI, EndROI;
        
        public Form1()
        {
            InitializeComponent();
            imgList = new Dictionary<string, Image<Bgr, byte>>();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    imgList.Clear();
                    treeView1.Nodes.Clear();

                    var img= new Image<Bgr, byte>(dialog.FileName);
                    AddImage(img, "Input");
                    pictureBox1.Image = img.AsBitmap();
                    cols= img.Width;
                    rows = img.Height;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void binarizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgList.Count==0)
                {
                    MessageBox.Show("Select and image to process");
                    return;
                }

                var bw = imgList["Input"]
                    .Convert<Gray, byte>()
                    .ThresholdBinary(new Gray(100), new Gray(255));

                AddImage(bw.Convert<Bgr, byte>(), "Binary");
                pictureBox1.Image = bw.AsBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                pictureBox1.Image = imgList[e.Node.Text].AsBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void gaussianBlurToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("Select an image to process");
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>().SmoothGaussian(3);
                pictureBox1.Image = img.AsBitmap();
                AddImage(img, "Gaussian");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddImage(Image<Bgr,byte> img, string keyName)
        {
            if (!treeView1.Nodes.ContainsKey(keyName))
            {
                TreeNode node = new TreeNode(keyName);
                node.Name = keyName;
                treeView1.Nodes.Add(node);
                treeView1.SelectedNode = node;
            }

            if (!imgList.ContainsKey(keyName))
            {
                imgList.Add(keyName, img);

            }
            else
            {
                imgList[keyName] = img;
            }
        }
        private void differenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (TreeNode item in treeView1.Nodes)
                {
                    if (item.Checked)
                    {
                        list.Add(item.Text);
                    }
                }

                if (list.Count>1)
                {
                    var img = new Mat();
                    CvInvoke.AbsDiff(imgList[list[0]], imgList[list[1]], img);
                    AddImage(img.ToImage<Bgr,byte>(), "Difference");
                    pictureBox1.Image = img.ToBitmap();

                }
                else
                {
                    MessageBox.Show("Select two images.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image==null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                var output = new Mat();
                double threshold = CvInvoke.Threshold(img.Convert<Gray,byte>(), output, 0, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);

                pictureBox1.Image = output.ToBitmap();
                AddImage(output.ToImage<Bgr, byte>(), "Ostu");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pyUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                var imgout =  img.PyrUp();

                pictureBox1.Image = imgout.ToBitmap();
                AddImage(imgout, "PyUp");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pyDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                var imgout = img.PyrDown();

                pictureBox1.Image = imgout.ToBitmap();
                AddImage(imgout, "PyDown");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void findOrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BGR 0 155  245     20  175  265

            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                if (img.NumberOfChannels>=3)
                {
                    var mask = img.InRange(new Bgr(0, 100, 240), new Bgr(180, 220, 255));
                    var imgout = img.Copy(mask);
                    pictureBox1.Image = imgout.ToBitmap();
                    AddImage(imgout, "Oranges");
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showPixelValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void findAppleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BGR 20 100  50     220  255  220

            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                if (img.NumberOfChannels >= 3)
                {
                    var mask = img.InRange(new Bgr(50, 100, 50), new Bgr(220, 255, 220));
                    var imgout = img.Copy(mask);
                    pictureBox1.Image = imgout.ToBitmap();
                    AddImage(imgout, "Oranges");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image==null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
                var hsv = img.Convert<Hsv, byte>();
                int epsilon = 30;
                var mask= hsv.InRange(new Hsv(60 - epsilon, 35, 0), new Hsv(60 + epsilon, 255, 255));
                var imgout = img.Copy(mask);
                pictureBox1.Image = imgout.ToBitmap();

                AddImage(imgout, "Apples");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void hitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>()
                    .ThresholdBinary(new Gray(100),new Gray(255));

                //Mat SE = Mat.Zeros(3, 3, Emgu.CV.CvEnum.DepthType.Cv8S,1);
                
                int[,] data = {
                    { 0, -1, -1 },
                    { 1, 1, -1 },
                    { 0, 1, 0 } };
                Matrix<int> SE = new Matrix<int>(data);
                
                Mat imgout = new Mat();
                CvInvoke.MorphologyEx(img, imgout, Emgu.CV.CvEnum.MorphOp.HitMiss, SE, new Point(0, 2), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0));

                pictureBox1.Image = imgout.ToBitmap();
                //var SE = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Custom,new Size(3,3),new Point(-1,-1));
                //SE.SetTo(tempStructure);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void filter2DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                float[,] data = {
                    { 0, 1, 0 },
                    { 1, -1, 1},
                    { 0, 1, 0 } };
                Matrix < float> SE = new Matrix<float>(data);
                SE._Mul(1/9.0);

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, float>();

                var imgout = img.CopyBlank();
                CvInvoke.Filter2D(img, imgout,SE,new Point(-1,-1));

                pictureBox1.Image = imgout.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void borderPaddingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, float>();

                var imgout = new Mat();
                CvInvoke.CopyMakeBorder(img, imgout, 5,5,5,5,Emgu.CV.CvEnum.BorderType.Constant,new MCvScalar(0));

                pictureBox1.Image = imgout.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void borderRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, float>();

                int width = img.Width;
                int height = img.Height;

                img.ROI = new Rectangle(5, 5, width - 10, height - 10);
                var imgout = img.Copy();
                img.ROI = Rectangle.Empty;

                pictureBox1.Image = imgout.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void binaryInverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>()
                    .Not();

                pictureBox1.Image = img.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                float[,] data = {
                    { -1, 0, 1 },
                    { -2, 0, 2},
                    { -1, 0, 1 } };
                Matrix<float> SEx = new Matrix<float>(data);

                Matrix<float> SEy = SEx.Transpose();

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, float>();

                var Gx = new Mat();
                var Gy = new Mat();

                CvInvoke.Sobel(img, Gx, Emgu.CV.CvEnum.DepthType.Cv32F,1,0);
                CvInvoke.Sobel(img, Gy, Emgu.CV.CvEnum.DepthType.Cv32F, 0, 1);

                var gx = Gx.ToImage<Gray, float>();
                var gy = Gy.ToImage<Gray, float>();

                var Gxx = new Mat(Gx.Size, Emgu.CV.CvEnum.DepthType.Cv32F, 1);
                var Gyy = new Mat(Gx.Size, Emgu.CV.CvEnum.DepthType.Cv32F, 1);

                CvInvoke.ConvertScaleAbs(Gx, Gxx, 0, 0);
                CvInvoke.ConvertScaleAbs(Gy, Gyy, 0, 0);
                
                var mag = new Mat(Gx.Size,Emgu.CV.CvEnum.DepthType.Cv8U,1);
                CvInvoke.AddWeighted(Gxx, 0.5, Gyy, 0.5, 0, mag);

                AddImage(mag.ToImage<Bgr,byte>(), "Mag Absolute");


                gx._Mul(gx);
                gy._Mul(gy);

                var M = new Mat(gx.Size,Emgu.CV.CvEnum.DepthType.Cv8U,1);
                CvInvoke.Sqrt(gx + gy,M);
                AddImage(M.ToImage<Bgr, byte>(), "Mag Squared");
                //CvInvoke.Filter2D(img, Gx, SEx, new Point(-1, -1));
                //CvInvoke.Filter2D(img, Gy, SEy, new Point(-1, -1));

                pictureBox1.Image = M.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showPixelValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPixelValue = !showPixelValue;
        }

        private void selectingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selecting = !selecting;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (selecting==true)
            {
                down = true;
                start = e.Location;
            }

            if (SelectROI)
            {
                down = true;
                StartROI = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (selecting)
            {
                selecting = false;
                down = false;
                end = e.Location;
            }

            if (SelectROI)
            {
                SelectROI = false;
                down = false;
                EndROI = e.Location;
                int width = Math.Abs(StartROI.Y - EndROI.Y);
                int height = Math.Abs(StartROI.X - EndROI.X);

                rectROI = new Rectangle(StartROI.X, StartROI.Y, width, height);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (selecting)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }

            if (down)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                    return;

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>();
                Mat hist = new Mat();
                float[] ranges = new float[] { 0, 256};
                int[] channel= { 0};
                int[] histSize = { 256};

                VectorOfMat ms = new VectorOfMat();
                ms.Push(img);
                CvInvoke.CalcHist(ms,channel, null, hist,histSize, ranges, false);

                HistogramViewer viewer = new HistogramViewer();
                viewer.Text = "Image Histogram";
                viewer.ShowIcon = false;
                viewer.HistogramCtrl.AddHistogram("Image Histogram", Color.Blue, hist,256,ranges);
                viewer.HistogramCtrl.Refresh();
                viewer.Show();

                
                //pictureBox1.Image = CreateGraph(hist).GetImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private ZedGraphControl CreateGraph(Mat data)
        {
            int[] arr = new int[data.Rows];
            data.CopyTo(arr);
            ZedGraphControl zgc = new ZedGraphControl();
           
            PointPairList list = new PointPairList();
            int x=0;
            for (int i= 0; i<arr.Length; i++)
            {
                list.Add( arr[i],x);
                x++;
            }
            GraphPane pane = zgc.GraphPane;
            pane.CurveList.Clear();

            //BurlyWood Coral DarkMagenta
            //LineItem myCurve = pane.AddCurve("Гистограмма", list, Color.Coral, SymbolType.None);
            //Color color = Color.FromArgb(100, Color.Coral);
            //myCurve.Line.Fill = new ZedGraph.Fill(color);
            pane.AddCurve("Circle", list, Color.Crimson, SymbolType.Circle);
            //pane.AddBar("Ожидаемый результат", list2, Color.CadetBlue);
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = arr.Max();

            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 255;

            pane.Title.Text = "Graph";
            zgc.AxisChange();
            zgc.Invalidate();
            return zgc;
        }

        private void equalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                    return;

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>();
                Mat histeq = new Mat();

                CvInvoke.EqualizeHist(img, histeq);
                CvInvoke.Imshow("Histogram Equalization", histeq);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                    return;

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>();

                Image<Gray, byte> img1=null;
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    img1 = new Image<Gray, byte>(dialog.FileName);
                }
                
                Mat hist = new Mat();
                Mat hist1 = new Mat();

                float[] ranges = new float[] { 0, 256 };
                int[] channel = { 0 };
                int[] histSize = { 256 };

                VectorOfMat ms = new VectorOfMat();
                ms.Push(img);

                VectorOfMat ms1 = new VectorOfMat();
                ms1.Push(img1);

           
                CvInvoke.CalcHist(ms, channel, null, hist, histSize, ranges, false);
                CvInvoke.CalcHist(ms1, channel, null, hist1, histSize, ranges, false);

                CvInvoke.Normalize(hist, hist);
                CvInvoke.Normalize(hist1, hist1);

                HistogramViewer viewer = new HistogramViewer();
                viewer.Text = "Image Histogram";
                viewer.ShowIcon = false;
                viewer.HistogramCtrl.AddHistogram("Image1 Histogram", Color.Blue, hist, 256, ranges);
                viewer.HistogramCtrl.Refresh();
                viewer.Show();

                HistogramViewer viewer1 = new HistogramViewer();
                viewer1.Text = "Image Histogram";
                viewer1.ShowIcon = false;
                viewer1.HistogramCtrl.AddHistogram("Image2 Histogram", Color.Blue, hist1, 256, ranges);
                viewer1.HistogramCtrl.Refresh();
                viewer1.Show();


                var result1 = CvInvoke.CompareHist(hist, hist, Emgu.CV.CvEnum.HistogramCompMethod.Correl);
                var result2 = CvInvoke.CompareHist(hist1, hist1, Emgu.CV.CvEnum.HistogramCompMethod.Correl);
                var result3 = CvInvoke.CompareHist(hist, hist1, Emgu.CV.CvEnum.HistogramCompMethod.Correl);

                lblBGR.Text = "Hist vs Hist = " + result1.ToString() +"\n"+
                    "Hist1 vs Hist1 = " + result2.ToString() + "\n"+
                    "Hist vs Hist1 = " + result3.ToString() + "\n";

                //pictureBox1.Image = CreateGraph(hist).GetImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void scharrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                float[,] data = {
                    { -3,  0, 3 },
                    { -10, 0, 10},
                    { -3,  0, 3 } };
                Matrix<float> SEx = new Matrix<float>(data);

                Matrix<float> SEy = SEx.Transpose();

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, float>();

                var Gx = new Mat();
                var Gy = new Mat();

                CvInvoke.Scharr(img, Gx, Emgu.CV.CvEnum.DepthType.Cv16S, 1, 0);
                CvInvoke.Scharr(img, Gy, Emgu.CV.CvEnum.DepthType.Cv16S, 0, 1);

                CvInvoke.ConvertScaleAbs(Gx, Gx, 0, 0);
                CvInvoke.ConvertScaleAbs(Gy, Gy, 0, 0);

                CvInvoke.Multiply(Gx, Gx, Gx);
                CvInvoke.Multiply(Gy, Gy, Gy);

                Gx.ConvertTo(Gx, Emgu.CV.CvEnum.DepthType.Cv32F);
                Gy.ConvertTo(Gy, Emgu.CV.CvEnum.DepthType.Cv32F);

                var M = new Mat(Gx.Size, Emgu.CV.CvEnum.DepthType.Cv32F, 1);
                CvInvoke.Sqrt(Gx + Gy, M);
                var imgout = M.ToImage<Bgr, byte>();
                AddImage(imgout, "Scharr");

                pictureBox1.Image = imgout.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backProjectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                    return;

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Gray, byte>();

                Image<Gray, byte> img1 = null;
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    img1 = new Image<Gray, byte>(dialog.FileName);
                }

                Mat hist = new Mat();
                Mat hist1 = new Mat();

                float[] ranges = new float[] { 0, 256 };
                int[] channel = { 0 };
                int[] histSize = { 256 };

                VectorOfMat ms = new VectorOfMat();
                ms.Push(img);

                VectorOfMat ms1 = new VectorOfMat();
                ms1.Push(img1);


                CvInvoke.CalcHist(ms, channel, null, hist, histSize, ranges, false);
                CvInvoke.CalcHist(ms1, channel, null, hist1, histSize, ranges, false);

                CvInvoke.Normalize(hist, hist);
                CvInvoke.Normalize(hist1, hist1);


                Mat proj = new Mat();
                CvInvoke.CalcBackProject(ms, channel, hist, proj, ranges);


                HistogramViewer viewer = new HistogramViewer();
                viewer.Text = "Image Histogram";
                viewer.ShowIcon = false;
                viewer.HistogramCtrl.AddHistogram("Image1 Histogram", Color.Blue, hist, 256, ranges);
                viewer.HistogramCtrl.Refresh();
                viewer.Show();

                HistogramViewer viewer1 = new HistogramViewer();
                viewer1.Text = "Image Histogram";
                viewer1.ShowIcon = false;
                viewer1.HistogramCtrl.AddHistogram("Image2 Histogram", Color.Blue, hist1, 256, ranges);
                viewer1.HistogramCtrl.Refresh();
                viewer1.Show();


               
                pictureBox1.Image = proj.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void convexHullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image==null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, byte>();

                img = img.SmoothGaussian(3);
                var gray = img.Convert<Gray, byte>()
                    .ThresholdBinaryInv(new Gray(225), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                Mat hier = new Mat();
                
                CvInvoke.FindContours(gray, contours, hier, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

                int index = -1;
                double maxarea = -100;
                for (int i = 0; i < contours.Size; i++)
                {
                  double area =   CvInvoke.ContourArea(contours[i]);
                    if (area>maxarea)
                    {
                        maxarea = area;
                        index = i;
                    }
                }

                if (index>-1)
                {
                    var biggestcontour = contours[index];
                    //Mat hull = new Mat();

                    VectorOfPoint hull = new VectorOfPoint();
                    CvInvoke.ConvexHull(biggestcontour, hull);
                    
                    //CvInvoke.DrawContours(img, hull, -1, new MCvScalar(0, 0, 255), 3);
                    CvInvoke.Polylines(img, hull.ToArray(), true, new MCvScalar(0, 0.0, 255.0),3);

                }
                pictureBox1.Image = img.AsBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void watershedSegmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, byte>();

                var mask = img.Convert<Gray, byte>()
                    .ThresholdBinaryInv(new Gray(210), new Gray(255))
                    .Dilate(2);
                //var img1 = img.Copy(mask);

                float[,] data = {
                    { 1, 1, 1 },
                    { 1, -8, 1},
                    { 1, 1, 1 } };
                Matrix<float> kernel = new Matrix<float>(data);

                Mat imgLaplacian = new Mat();
                CvInvoke.Filter2D(img, imgLaplacian,kernel, new Point(-1,-1));
                Mat sharp = new Mat();
                img.Mat.ConvertTo(sharp,Emgu.CV.CvEnum.DepthType.Cv32S);

                Mat imgResult = sharp - imgLaplacian;
                // convert back to 8bits gray scale
                imgResult.ConvertTo(imgResult, Emgu.CV.CvEnum.DepthType.Cv8U);
                imgLaplacian.ConvertTo(imgLaplacian,Emgu.CV.CvEnum.DepthType.Cv8U);


                Mat dimg = new Mat();
                Mat labels = new Mat();
                CvInvoke.DistanceTransform(mask, dimg, labels, Emgu.CV.CvEnum.DistType.L2, 3);

                CvInvoke.Normalize(dimg, dimg, normType: Emgu.CV.CvEnum.NormType.MinMax);


                CvInvoke.Threshold(dimg, dimg, 0.8, 1.0, Emgu.CV.CvEnum.ThresholdType.Binary);

                var markers = new Mat(imgLaplacian.Size, Emgu.CV.CvEnum.DepthType.Cv32S, 1);
                dimg.ConvertTo(markers,Emgu.CV.CvEnum.DepthType.Cv32S);

                CvInvoke.Watershed(imgLaplacian,  markers);

                Mat imgout = new Mat();
                markers.ConvertTo(imgout, Emgu.CV.CvEnum.DepthType.Cv8U);



                pictureBox1.Image = imgout.ToImage<Gray,byte>().Mul(255).ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void logoDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image== null)
                {
                    return;
                }
                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, byte>();

                Image<Bgr, byte> logo = null;
                OpenFileDialog dialog = new OpenFileDialog();

                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    logo = new Image<Bgr, byte>(dialog.FileName);
                }

                var vp = Process(logo.Mat, img.Mat);

                if (vp != null)
                {
                    CvInvoke.Polylines(img, vp, true, new MCvScalar(0, 0, 255), 5);
                }
                pictureBox1.Image = img.AsBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static VectorOfPoint Process(Mat logo, Mat observedImage)
        {
            VectorOfPoint vp = null;
            Mat homography = null;
            VectorOfKeyPoint logoKeyPoints = new VectorOfKeyPoint();
            VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
            Mat mask;   
            int k = 2;
            double uniquenessThreshold = 0.80;

            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                using (UMat uModelImage = logo.GetUMat(AccessType.Read))
                using (UMat uObservedImage = observedImage.GetUMat(AccessType.Read))
                {
                    KAZE featureDetector = new KAZE();

                    //extract features from the object image
                    Mat modelDescriptors = new Mat();
                    featureDetector.DetectAndCompute(uModelImage, null, logoKeyPoints, modelDescriptors, false);


                    // extract features from the observed image
                    Mat observedDescriptors = new Mat();
                    featureDetector.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);

                    // Bruteforce, slower but more accurate
                    // You can use KDTree for faster matching with slight loss in accuracy
                    using (Emgu.CV.Flann.LinearIndexParams ip = new Emgu.CV.Flann.LinearIndexParams())
                    using (Emgu.CV.Flann.SearchParams sp = new SearchParams())
                    using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
                    {
                        matcher.Add(modelDescriptors);

                        matcher.KnnMatch(observedDescriptors, matches, k, null);
                        mask = new Mat(matches.Size, 1, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                        mask.SetTo(new MCvScalar(255));
                        Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                        int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(logoKeyPoints, observedKeyPoints,
                            matches, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                        {
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(logoKeyPoints,
                            observedKeyPoints, matches, mask, 2);
                        }
                    }

                }

                if (homography != null)
                {
                    //draw a rectangle along the projected model
                    Rectangle rect = new Rectangle(Point.Empty, logo.Size);
                    PointF[] pts = new PointF[]
                    {
                     new PointF(rect.Left, rect.Bottom),
                     new PointF(rect.Right, rect.Bottom),
                     new PointF(rect.Right, rect.Top),
                     new PointF(rect.Left, rect.Top)
                    };
                    
                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                    vp = new VectorOfPoint(points);
                }
                return vp;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void selectROIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                SelectROI = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void processToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                ProcessROI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProcessROI()
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                if (rect == null)
                {
                    MessageBox.Show("Select an ROI");
                    return;
                }

                var img = new Bitmap(pictureBox1.Image)
                   .ToImage<Bgr, byte>();

                img.ROI = rect;
                var img2 = img.Copy();
                var img3 = img2.SmoothGaussian(25);

               
                //var blue = img2.SmoothGaussian(15).Canny(150, 50)
                //Image<Bgr, byte> img3 = new Image<Bgr, byte>(new Image<Gray, byte>[] {
                //blue,
                //blue,
                //blue });

                img.SetValue(new Bgr(1, 1, 1));
                img._Mul(img3);

                img.ROI = Rectangle.Empty;
                pictureBox1.Image = img.AsBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void templateMatchinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                if (rect==null)
                {
                    MessageBox.Show("Select a template");
                    return;
                }

                var img = new Bitmap(pictureBox1.Image)
                    .ToImage<Bgr, byte>();

                img.ROI = rect;
                var template = img.Copy();
                //template = template.Resize(0.5, Emgu.CV.CvEnum.Inter.Cubic);
                template = template.Rotate(90, new Bgr(0, 0, 0));
                img.ROI = Rectangle.Empty;

                CvInvoke.Imshow("Template", template);

                Mat imgout = new Mat();
                CvInvoke.MatchTemplate(img, template, imgout, Emgu.CV.CvEnum.TemplateMatchingType.Sqdiff);

                double minVal=0; 
                double maxVal=0; 
                Point minLoc = new Point(); 
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(imgout, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                Rectangle r = new Rectangle(minLoc, template.Size);
                CvInvoke.Rectangle(img, r,new  MCvScalar(255,0,0), 3);
                pictureBox1.Image = img.AsBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }

                if (showPixelValue)
                {
                    if (e.X<cols && e.Y<rows)
                    {
                        Bgr bgr = imgList["Input"][e.Y, e.X];
                        lblBGR.Text ="B G R: " +  bgr.Blue + " " + bgr.Green + " " + bgr.Red;
                    }
                }
                if (selecting && down)
                {
                    int x = Math.Min(start.X, e.X);
                    int y = Math.Min(start.Y, e.Y);

                    int width = Math.Max(start.X, e.X) - Math.Min(start.X, e.X);

                    int height = Math.Max(start.Y, e.Y) - Math.Min(start.Y, e.Y);
                    rect = new Rectangle(x, y, width, height);
                    Refresh();
                }

                if (SelectROI)
                {
                    int x = Math.Min(StartROI.X, e.X);
                    int y = Math.Min(StartROI.Y, e.Y);

                    int width = Math.Max(StartROI.X, e.X) - Math.Min(StartROI.X, e.X);

                    int height = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
                    rect = new Rectangle(x, y, width, height);
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
