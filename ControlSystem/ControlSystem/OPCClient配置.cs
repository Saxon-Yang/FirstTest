using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ControlSystem
{
    public partial class OPCClient配置 : Form
    {

        Thread th = null;
        string IP = "";
        string OPCName = "";
        OPCServer opcServer = null;
        string[] itemNames = new string[10];
        string[] getedValues = { };
        List<string> outitemNames = null;
        OPCItem outopcItem = null;
        List<int> outitemClientHandle = null;
        Dictionary<string, string> itemValues = null;

        OPCGroup opcGroup = null;


        public OPCClient配置()
        {
            InitializeComponent();

            //itemNames = new string[] { "Simulation Examples.Functions.Random1", "Simulation Examples.Functions.Random2",
            //"Simulation Examples.Functions.Random3","Simulation Examples.Functions.Random4"};

            for (int i = 0; i < 10; i++)
            {
                itemNames[i] = "Simulation Examples.Functions.Random" + (i + 1).ToString();
            }
            //预设点  及预设IP地址
            tbx_ip.Text = "10.1.50.126";
            tbx_name.Text = "Kepware.KEPServerEX.V5";
            //for (int i = 0; i < 4000; i++)
            //{
            //    lbx_pointlist.Items.Add("point.AAAAAA.aaaaaa.ccccccc.cccccc.ccccccc"+i);  
            //}
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Form_Add form = new Form_Add();
            if (form.ShowDialog() == DialogResult.OK)
            {
                lbx_pointlist.Items.Add(form.PointName);
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (lbx_pointlist.SelectedItem != null)
            {
                lbx_pointlist.Items.Remove(lbx_pointlist.SelectedItem);
            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            btn_connect.Enabled = false;
            btn_add.Enabled = false;
            btn_del.Enabled = false;

            if (tbx_ip.Text != "" && tbx_name.Text != "")
            {
                IP = tbx_ip.Text;
                OPCName = tbx_name.Text;
                th = new Thread(new ThreadStart(Run));
                th.Start();
            }
            else
            {
                MessageBox.Show("服务器IP和名称不能为空！");
            }

        }

        private void Run()
        {
            OPCHelp opcHelp = new OPCHelp();
            opcServer = opcHelp.ConnectOPCServer(OPCName, IP);
            if (opcServer != null)
            {
                //string info = opcHelp.GetServerInfo(opcServer);
                //List<string> nodeList = opcHelp.RecurBrowse(opcHelp.CreateOPCBrowser(opcServer));
                Invoke((EventHandler)delegate
                {
                    rtb_msg.AppendText("连接服务器成功！\r\n");
                    //for (int i = 0; i < nodeList.Count; i++)
                    //{
                    //    rtb_msg.AppendText(nodeList[i].ToString() + "\r\n");
                    //}
                });
            }
            opcGroup = opcHelp.CreateGroup(opcServer);
            if (opcGroup != null)
            {
                OPCItems opcItems = opcHelp.GetOpcItems(opcGroup);
                opcHelp.SetGroupProperty(opcGroup, 3000, new DIOPCGroupEvent_DataChangeEventHandler(opcGroup_DataChanged));
                itemValues = opcHelp.AddItems(itemNames, opcItems, out outopcItem, out outitemNames, out outitemClientHandle);
                opcHelp.GetItemValues(itemNames, itemValues);
            }
        }
        private void opcGroup_DataChanged(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            Thread.Sleep(2000);
            for (int i = 1; i < NumItems + 1; i++)
            {

                itemValues[itemNames[i - 1]] = ItemValues.GetValue(i).ToString();
                Console.WriteLine(itemValues[itemNames[i - 1]]);
                //Invoke((EventHandler)delegate
                //{
                //    rtb_msg.AppendText("获得点" + itemNames[i - 1] + "的值为：" + itemValues[itemNames[i - 1]] + "\r\n");
                //    if (rtb_msg.TextLength > 3500)
                //    {
                //        rtb_msg.Clear();
                //    }
                //});
            }
           
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            btn_connect.Enabled = true;
            btn_add.Enabled = true;
            btn_del.Enabled = true;

            if (opcServer != null)
            {
                opcServer.Disconnect();
                opcServer = null;
                rtb_msg.AppendText("断开OPCServer连接成功\r\n");
                th.Abort();
            }
            else
            {
                rtb_msg.AppendText("连接已断开\r\n");
                th.Abort();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (opcServer != null)
            //{
            //    int state = opcServer.ServerState;
            //    rtb_msg.AppendText("连接状态：" + state + "\r\n");
            //}


        }
    }
}
