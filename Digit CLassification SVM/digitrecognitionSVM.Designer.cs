namespace Emgucv33Apps
{
    partial class digitrecognitionSVM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainSVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbShowData = new System.Windows.Forms.CheckBox();
            this.lblTrain = new System.Windows.Forms.Label();
            this.lblTest = new System.Windows.Forms.Label();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.lblOouputLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sVMToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDataToolStripMenuItem
            // 
            this.loadDataToolStripMenuItem.Name = "loadDataToolStripMenuItem";
            this.loadDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadDataToolStripMenuItem.Text = "Load Data";
            this.loadDataToolStripMenuItem.Click += new System.EventHandler(this.loadDataToolStripMenuItem_Click);
            // 
            // sVMToolStripMenuItem
            // 
            this.sVMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainSVMToolStripMenuItem,
            this.testSVMToolStripMenuItem});
            this.sVMToolStripMenuItem.Name = "sVMToolStripMenuItem";
            this.sVMToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.sVMToolStripMenuItem.Text = "SVM";
            // 
            // trainSVMToolStripMenuItem
            // 
            this.trainSVMToolStripMenuItem.Name = "trainSVMToolStripMenuItem";
            this.trainSVMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.trainSVMToolStripMenuItem.Text = "Train SVM";
            this.trainSVMToolStripMenuItem.Click += new System.EventHandler(this.trainSVMToolStripMenuItem_Click);
            // 
            // testSVMToolStripMenuItem
            // 
            this.testSVMToolStripMenuItem.Name = "testSVMToolStripMenuItem";
            this.testSVMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.testSVMToolStripMenuItem.Text = "Test SVM";
            this.testSVMToolStripMenuItem.Click += new System.EventHandler(this.testSVMToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(23, 53);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 97);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // cbShowData
            // 
            this.cbShowData.AutoSize = true;
            this.cbShowData.Location = new System.Drawing.Point(23, 28);
            this.cbShowData.Name = "cbShowData";
            this.cbShowData.Size = new System.Drawing.Size(85, 17);
            this.cbShowData.TabIndex = 2;
            this.cbShowData.Text = "Show Image";
            this.cbShowData.UseVisualStyleBackColor = true;
            this.cbShowData.CheckedChanged += new System.EventHandler(this.cbShowData_CheckedChanged);
            // 
            // lblTrain
            // 
            this.lblTrain.AutoSize = true;
            this.lblTrain.Location = new System.Drawing.Point(148, 32);
            this.lblTrain.Name = "lblTrain";
            this.lblTrain.Size = new System.Drawing.Size(7, 13);
            this.lblTrain.TabIndex = 3;
            this.lblTrain.Text = "\r\n";
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Location = new System.Drawing.Point(151, 53);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(7, 13);
            this.lblTest.TabIndex = 4;
            this.lblTest.Text = "\r\n";
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Location = new System.Drawing.Point(148, 112);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(7, 13);
            this.lblAccuracy.TabIndex = 5;
            this.lblAccuracy.Text = "\r\n";
            // 
            // lblOouputLabel
            // 
            this.lblOouputLabel.AutoSize = true;
            this.lblOouputLabel.Location = new System.Drawing.Point(148, 85);
            this.lblOouputLabel.Name = "lblOouputLabel";
            this.lblOouputLabel.Size = new System.Drawing.Size(7, 13);
            this.lblOouputLabel.TabIndex = 6;
            this.lblOouputLabel.Text = "\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "<|-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(65, 156);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "-|>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // digitrecognitionSVM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 200);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblOouputLabel);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.lblTrain);
            this.Controls.Add(this.cbShowData);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "digitrecognitionSVM";
            this.Text = "digitrecognitionSVM";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sVMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainSVMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSVMToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbShowData;
        private System.Windows.Forms.Label lblTrain;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.Label lblOouputLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
