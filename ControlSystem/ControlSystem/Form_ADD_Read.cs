using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlSystem
{
    public partial class Form_ADD_Read : Form
    {
        public string Topic { get; set; }
        public Form_ADD_Read()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Topic = tbx_add_read.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
