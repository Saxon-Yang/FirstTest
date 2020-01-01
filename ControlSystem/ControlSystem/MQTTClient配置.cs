using System;
using System.Threading;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ControlSystem
{
    public partial class MQTTClient配置 : Form
    {
        private delegate void ShowMessage(string msg);
        private ShowMessage showMessage;
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
            btn_connect.Enabled = false;
            if (tbx_ip.Text != null)
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
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
                client.Subscribe(new string[] { "tttt" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            }
            catch (Exception ex)
            {
                btn_connect.Enabled = true;
                MessageBox.Show(ex.Message);
            }
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msg = System.Text.Encoding.UTF8.GetString(e.Message);
            BeginInvoke(showMessage = xx =>
            {
                rtb_msg.AppendText("收到" + e.Topic + "消息：" + xx + "\r\n");
            }, msg
            );
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {


            if (client != null)
            {
                th.Abort();
                client.Disconnect();
                client = null;
                btn_add_read.Enabled = true;
                btn_add_write.Enabled = true;
                btn_del_read.Enabled = true;
                btn_del_write.Enabled = true;
                btn_connect.Enabled = true;
                rtb_msg.AppendText("断开与MqttServer连接！\r\n");
            }
            else
            {
                rtb_msg.AppendText("与MqttServer连接已断开！\r\n");
            }


            
        }

        private void btn_add_read_Click(object sender, EventArgs e)
        {
            Form_ADD_Read fm = new Form_ADD_Read();
            if (fm.ShowDialog()==DialogResult.OK)
            {
                listBoxControl1.Items.Add(fm.Topic);
            }
        }
    }
}
