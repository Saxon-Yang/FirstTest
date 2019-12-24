using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection
{
    class Program
    {
        private static OPCServer opcServer=null;
        static void Main(string[] args)
        {
            //从配置文件中读取opcServer 名称，IP
            string opcServerName = ConfigurationManager.AppSettings["opcServerName"].ToString();
            string opcServerIP= ConfigurationManager.AppSettings["opcServerIP"].ToString();

            OPCHelp opcHelper = new OPCHelp();
            opcServer=opcHelper.ConnectOPCServer(opcServerName, opcServerIP);
            if (opcServer!=null)
            {
                Console.WriteLine("连接OPCServer成功！");
            }
            Console.ReadKey();
        }
    }
}
