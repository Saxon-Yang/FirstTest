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
    public partial class OPCClient配置 : Form
    {
        public OPCClient配置()
        {
            InitializeComponent();

            for (int i = 0; i < 1500; i++)
            {
                lbx_pointlist.Items.Add("point"+i);  
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Form_Add form = new Form_Add();
            if (form.ShowDialog()==DialogResult.OK)
            {
                lbx_pointlist.Items.Add(form.PointName);
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (lbx_pointlist.SelectedItem!=null)
            {
                lbx_pointlist.Items.Remove(lbx_pointlist.SelectedItem);
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_connect.Enabled = false;
            btn_add.Enabled = false;
            btn_del.Enabled = false;

            OPCHelp opcHelp = new OPCHelp();

        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            btn_connect.Enabled = true;
            btn_add.Enabled = true;
            btn_del.Enabled = true;
        }
    }
}
