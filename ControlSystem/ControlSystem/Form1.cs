using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace ControlSystem
{
    public partial class Form1 : Form
    {

        public static Dictionary<string, int> index = new Dictionary<string, int>();
        public static int count = 0;
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
                index.Add(name, count++);
            }
            else if (form!=null &&isOpen!=null)
            {
                xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[index[name]];
            }
        }
    }
}
