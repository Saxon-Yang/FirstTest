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
    public partial class Form_Add : Form
    {
        public string PointName { get; set; }
        public Form_Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbx_addName!=null|| tbx_addName.Text!="")
            {
                PointName = tbx_addName.Text;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("点名称不能为空！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
