using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ossuploadtool
{
    public partial class uploadeds : Form
    {

        public string InputText = "";

        public uploadeds()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text);
        }

        private void uploadeds_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = this.InputText;
        }
    }
}
