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
                OPCGroup group = groups.Add("OPCDotNetGroup");
                return group;
            }
            catch (Exception err)
            {

                Console.WriteLine("创建OPC组失败" + err.Message);
                return null;
            }
        }

        public OPCGroup SetGroupProperty(OPCGroup opcGroup, int UpdateRate)
        {
            try
            {
                opcGroup.IsActive = true;
                opcGroup.DeadBand = 0;
                opcGroup.UpdateRate = UpdateRate;
                opcGroup.IsSubscribed = true;
                //opcGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(opcGroup_DataChanged);
                //opcGroup.AsyncWriteComplete += new DIOPCGroupEvent_AsyncWriteCompleteEventHandler(opcGroup_AsyncWriteComplete);
                return opcGroup;
            }
            catch (Exception err)
            {
                Console.WriteLine("设置组属性出错" + err.Message);
                return null;
               
            }
        }

    }
}
