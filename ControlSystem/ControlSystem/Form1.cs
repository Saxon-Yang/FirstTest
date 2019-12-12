using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
    
        }

        private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            Form test = Application.OpenForms["SetOpc"];
            if ((test==null)||(test.IsDisposed))
            {
                SetOpc form = new SetOpc();
                form.MdiParent = this;
                form.Show();
            }
            else
            {
                test.Activate();
            }
            
        }

      
    }
}
