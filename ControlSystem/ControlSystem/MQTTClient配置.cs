using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace ControlSystem
{
    public partial class MQTTClient配置 : Form
    {
        string IP = "";
        string PassWord = "";
        Thread th = null;
        MqttClient client = null;
        public MQTTClient配置()
        {
            InitializeComponent();
        }

        private void MQTTClient配置_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.count -= 1;
            Form1.index.Remove("MQTTClient配置");
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_add_read.Enabled = false;
            btn_add_write.Enabled = false;
            btn_del_read.Enabled = false;
            btn_del_write.Enabled = false;
            btn_disconnect.Enabled = false;
            if (tbx_ip.Text!=null)
            {
                IP = tbx_ip.Text;

                th = new Thread(new ThreadStart(Run));
                th.Start();
            }
            else
            {
                MessageBox.Show("MqttServer服务器的地址不能为空！");
            }
        }
        private void Run()
        {
            try
            {
                client = new MqttClient(IP);
                client.Connect()
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
