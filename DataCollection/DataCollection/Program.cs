using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollection
{
    class Program
    {
        public static Dictionary<string, string> itemvalues = new Dictionary<string, string>();
        private static OPCServer opcServer=null;
        private static OPCGroup opcGroup = null;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string[] EndName = {
                "ArgonFlow","AvgSLSpeed","BottomHeater","BottomHeaterCurrent","BottomHeaterStatus","CrownPixel",
                "CruciblelLift","CruciblePos","CrucibleRotation","CrystalID","CrystalID6","CrystalIDin",
                "CrystalLength","CrystalPos","CrystalWeight","Diameter","DiameterPixel","FastInflationV4",
                "FeederArgonFlow","FeederPressure","FeederRemainWeight","FeederSetArgonFlow","FeederValveClose","FeederValveOpen",
                "FeederVibration","FeedQuantity","FunctionalState","FurnanceCoverClose","IsolationValveClose","IsolationValveOpen",
                "LeakageRate","LowerArgonV6","MainHeater","MainHeaterCurrent","MainHeaterStatus","MainPressure_1000mT",
                "MainPressure_100T","MainPumpStatus","MainPumpV1","ManualSeed","MeltLevel","MeltLevelCoefficient",
                "MeltLevelPixel","MeltSurfTemp","PumpFrequency","RemainWeight","RunTime","ScadaConnection",
                "SeedLift","SeedRotation","SetBottomHeater","SetDiameter","SetMainHeater","SetMeltLevel",
                "SetMeltSurfTemp","SetPumpFrequency","SetSLSpeed","State","SubPressure_1000T","SubPumpStatus",
                "SubPumpV3","Temp","TotalFeedQuantity","UnlimitedFeedQuantity","UpperArgonV5"
            };

            string[] allName = new string[EndName.Length];
            List<string> itemName = new List<string>();
            for (int i = 0; i < 64; i++)
            {

                for (int j = 0; j < EndName.Length; j++)
                {
                    if (i < 9)
                    {
                        allName[j] = "Omron_I0" + (i + 1) + "." + "I0" + (i + 1) + "."+ EndName[j];
                        itemName.Add(allName[j]);
                    }
                    else
                    {
                        allName[j] = "Omron_I" + (i + 1) + "." + "I" + (i + 1) + "."+EndName[j];
                        itemName.Add(allName[j]);
                    }
                }

                
          
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine("-------初始化点完成------");
            Console.WriteLine("-------初始化"+(64*65)+"个点用时"+ts.TotalSeconds+"s"+"------");
            //从配置文件中读取opcServer 名称，IP
            string opcServerName = ConfigurationManager.AppSettings["opcServerName"].ToString();
            string opcServerIP= ConfigurationManager.AppSettings["opcServerIP"].ToString();

            OPCHelp opcHelper = new OPCHelp();
            opcServer=opcHelper.ConnectOPCServer(opcServerName, opcServerIP);
            if (opcServer!=null)
            {
                Console.WriteLine("连接OPCServer成功！");
            }

            opcGroup = opcHelper.CreateGroup(opcServer,"group1");

            opcGroup.IsActive = true;
            opcGroup.IsSubscribed = true;
            opcGroup.UpdateRate = 200;




            OPCItem[] item = AddItem(itemName, opcGroup);

            sw.Start();
            for (int i = 0; i < item.Length; i++)
            {
                itemvalues.Add(item[i].ItemID, Convert.ToString(item[i].Value));
            }
            sw.Stop();
            ts = sw.Elapsed;
            Console.WriteLine("所有值获取完成，花费时间" + ts.TotalSeconds + "s");
            Console.ReadKey();
        }


        private static OPCItem[] AddItem(List<string> item,OPCGroup opcGroup)
        {
            OPCItem[] opcItem = new OPCItem[item.Count];
            for (int i = 0; i < item.Count; i++)
            {
                opcItem[i]= opcGroup.OPCItems.AddItem(item[i],1);
            }
            return opcItem;
        }
    }
}
