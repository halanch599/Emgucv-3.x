using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageOverlay
{
    public partial class formParameters : Form
    {
        Form1 form;

        public formParameters(Form1 f)
        {
            InitializeComponent();
            form = f;
        }

        private void formParameters_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form!=null)
            {
                form.ApplyRangeFitler(trackBar1.Value, trackBar2.Value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
