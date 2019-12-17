using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ControlSystem
{
    public class OPCHelp
    {
        private OPCServer CreateOPCServer()
        {
            try
            {
                OPCServer opcServer = new OPCServer();
                return opcServer;
            }
            catch (Exception err)
            {
                Console.WriteLine("创建服务器失败!" + err.Message);
                return null;
            }
        }

        public string GetServerInfo(OPCServer opcServer)
        {
            try
            {
                return opcServer.StartTime.ToString() + "." + opcServer.MajorVersion.ToString() + "." + opcServer.MinorVersion.ToString() + "." + opcServer.BuildNumber.ToString();

            }
            catch (Exception err)
            {

                Console.WriteLine("获取服务器信息异常" + err.Message);
                return null;
            }
        }

        public OPCServer ConnectOPCServer(string strName, string strIP)
        {
            OPCServer tmp = CreateOPCServer();
            try
            {

                tmp.Connect(strName, strIP);
                if (tmp.ServerState != (int)OPCServerState.OPCRunning)
                {
                    tmp.Disconnect();
                    return null;
                }
                return tmp;
            }
            catch (Exception err)
            {
                tmp.Disconnect();
                Console.WriteLine("连接服务器出错" + err.Message);
                return null;
            }
        }

        public string GetLocalName()
        {
            return Dns.GetHostName();
        }

        public List<string> GetLocalIP()
        {
            List<string> ipAddress = new List<string>();
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress item in addresses)
            {
                ipAddress.Add(item.ToString());
            }
            return ipAddress;
        }

        public List<string> GetOpcServerName(string strHostName)
        {
            List<String> opcServerList = new List<string>();
            try
            {
                object serverlist = CreateOPCServer().GetOPCServers(strHostName);
                foreach (string item in (Array)serverlist)
                {
                    opcServerList.Add(item);
                }
                return opcServerList;
            }
            catch (Exception err)
            {

                Console.WriteLine("枚举本地OPC服务器出错:" + err.Message);
                return null;
            }
        }

        public OPCGroup CreateGroup(OPCServer opcServer)
        {
            try
            {
                OPCGroups groups = opcServer.OPCGroups;
                OPCGroup group = groups.Add("OPCDotNetGroup1");
                return group;
            }
            catch (Exception err)
            {

                Console.WriteLine("创建OPC组失败" + err.Message);
                return null;
            }
        }

        public OPCGroup SetGroupProperty(OPCGroup opcGroup, int UpdateRate, DIOPCGroupEvent_DataChangeEventHandler e)
        {
            try
            {
                opcGroup.IsActive = true;
                opcGroup.DeadBand = 0;
                opcGroup.UpdateRate = UpdateRate;
                opcGroup.IsSubscribed = true;
                opcGroup.DataChange +=e;
                return opcGroup;
            }
            catch (Exception err)
            {
                Console.WriteLine("设置组属性出错" + err.Message);
                return null;               
            }
        }
        public OPCItems GetOpcItems(OPCGroup opcGroup)
        {
            try
            {
                return opcGroup.OPCItems;
            }
            catch (Exception err)
            {
                Console.WriteLine("获取opc Items 出错" + err.Message);
                return null;
            }
        }

        public Dictionary<string,string>AddItems(string[]itemName,OPCItems opcItems,out OPCItem opcItem,out List<string>itemNames,out List<int> itemHandleClient)
        {
            itemNames = new List<string>();
            Dictionary<string, string> itemValues = new Dictionary<string, string>();
            itemHandleClient = new List<int>();
            opcItem = null;
            for (int i = 0; i < itemName.Length; i++)
            {
                itemNames.Add(itemName[i]);
                itemValues.Add(itemName[i],"");
            }
            for (int i = 0; i < itemName.Length; i++)
            {
                itemHandleClient.Add(itemHandleClient.Count!=0?itemHandleClient[itemHandleClient.Count-1]+1:1);
                opcItem = opcItems.AddItem(itemName[i],itemHandleClient[itemHandleClient.Count-1]);
            }
            return itemValues;

        }
        public bool Contains(string itemName, Dictionary<string, string> itemValues)
        {
            foreach (string item in itemValues.Keys)
            {
                if (item == itemName)
                {
                    return true;
                }
            }
            return false;
        }

        public string []GetItemValues(string []getItemNames,Dictionary<string,string>itemValues)
        {
            string[] getedValues = new string[getItemNames.Length];
            for (int i = 0; i < getItemNames.Length; i++)
            {
                if (Contains(getedValues[i], itemValues))
                {
                    itemValues.TryGetValue(getItemNames[i], out getedValues[i]);
                }
            }
            return getedValues;
        }
        public OPCBrowser CreateOPCBrowser(OPCServer opcServer)
        {
            try
            {
                return opcServer.CreateBrowser();
            }
            catch (Exception err)
            {

                Console.WriteLine("创建opcBrowser出错" + err.Message);
                return null;
            }
        }

        public List<string> RecurBrowse(OPCBrowser opcBrowser)
        {
            List<string> opcNodeNameList = new List<string>();

            //展开分支
            opcBrowser.ShowBranches();
            //展开叶子
            opcBrowser.ShowLeafs(true);
            foreach (object item in opcBrowser)
            {
                opcNodeNameList.Add(item.ToString());
            }
            return opcNodeNameList;
        }
    }
}
