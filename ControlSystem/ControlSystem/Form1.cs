using System;
using System.Reflection;
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
            string name = e.Link.Item.Caption;
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Form form = (Form)assembly.CreateInstance(type.Namespace + "." + name);
            Form isOpen= Application.OpenForms[name];
            if (form != null&& ((isOpen == null) || (isOpen.IsDisposed)))
            {
                form.Text = name;
                form.MdiParent = this;
                form.Show();
            }
            else if (form!=null &&isOpen!=null)
            {
                form.Activate();
            }
        }
    }
}
