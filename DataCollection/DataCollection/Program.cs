using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DataCollection
{
    class Program
    {
        public static string nowtime = DateTime.Now.ToString();
        public static Dictionary<string, string> itemvalues = new Dictionary<string, string>();
        public static Dictionary<string, string> itemvaluesTempOld = new Dictionary<string, string>();
        public static Dictionary<string, string> itemvaluesTemp;


        public static List<double> stepChangeState = new List<double>();
        public static List<double> stepChangeStateCompare = new List<double>();

        public static string oraStr = "";
        public static List<string> itemNames = new List<string>();
        private static OPCServer opcServer = null;
        private static OPCGroup opcGroup = null;
        static void Main(string[] args)
        {


            Thread th1 = new Thread(InsertTaskOnTime);
            th1.Start();
            th1.IsBackground = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            string[] EndName = {
                "Diameter","SetDiameter","Temp","MainHeater","SetMainHeater","BottomHeater",
                "SetBottomHeater","AvgSLSpeed","SetSLSpeed","MeltSurfTemp","SetMeltSurfTemp","MeltLevel",
                "SetMeltLevel","SeedLift","CruciblelLift","SeedRotation","CrucibleRotation","ArgonFlow",
                "MainPressure_100T","MainPressure_1000mT","SubPressure_1000T","DiameterPixel","MeltLevelPixel","CrystalLength",
                "CruciblePos","CrystalPos","CrystalWeight","RemainWeight","FeederRemainWeight","FeederVibration",
                "FeederSetArgonFlow","FeederArgonFlow","PumpFrequency","MainHeaterCurrent","BottomHeaterCurrent","RunTime",
                "FeedQuantity","UnlimitedFeedQuantity","TotalFeedQuantity","MeltLevelCoefficient","CrownPixel","LeakageRate",
                "SetPumpFrequency","State","CrystalID","MainPumpStatus","SubPumpStatus","MainHeaterStatus",
                "BottomHeaterStatus","ManualSeed","IsolationValveOpen","IsolationValveClose","MainPumpV1","SubPumpV3",
                "FastInflationV4","UpperArgonV5","LowerArgonV6","FeederValveOpen","FeederValveClose","FurnanceCoverClose",
                "FunctionalState","FeederPressure","CrystalID6","UltimateVacuum","HotCheackedLeakageRate"
            };

            string[] allName = new string[EndName.Length];
            for (int i = 93; i < 95; i++)       //遍历炉台
            {

                for (int j = 0; j < EndName.Length; j++)
                {
                    if (i < 9)
                    {
                        allName[j] = "Omron_I0" + (i + 1) + "." + "I0" + (i + 1) + "." + EndName[j];
                        itemNames.Add(allName[j]);
                    }
                    else
                    {
                        allName[j] = "Omron_I" + (i + 1) + "." + "I" + (i + 1) + "." + EndName[j];
                        itemNames.Add(allName[j]);
                    }
                }
            }

            for (int i = 0; i < itemNames.Count; i++)
            {
                itemvalues.Add(itemNames[i], "");
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine("-------初始化点完成------");
            Console.WriteLine("-------初始化点用时" + ts.TotalSeconds + "s" + "------");



            #region 测试代码

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < 4160; i++)
            //{
            //    itemnames.Add("Simulation Examples.Functions.Random" + (i + 1).ToString());

            //}
            //for (int i = 0; i < 4160; i++)
            //{
            //    itemvalues.Add(itemnames[i], "");
            //}

            ////for (int i = 0; i < itemnames.Count; i++)
            ////{
            ////    Console.WriteLine(itemnames[i]);
            ////}
            //sw.Stop();
            //TimeSpan ts = sw.Elapsed;
            //Console.WriteLine("点初始化完成，用时" + ts.TotalSeconds);
            #endregion
            //从配置文件中读取opcServer 名称，IP
            string opcServerName = ConfigurationManager.AppSettings["opcServerName"].ToString();
            string opcServerIP = ConfigurationManager.AppSettings["opcServerIP"].ToString();


            oraStr = ConfigurationManager.ConnectionStrings["OracleStr"].ConnectionString;

            OPCHelp opcHelper = new OPCHelp();
            opcServer = opcHelper.ConnectOPCServer(opcServerName, opcServerIP);
            if (opcServer != null)
            {
                Console.WriteLine("连接OPCServer成功！");
            }

            opcGroup = opcHelper.CreateGroup(opcServer, "group1");

            opcGroup.IsActive = true;
            opcGroup.IsSubscribed = true;
            opcGroup.UpdateRate = 200;


            sw.Start();

            OPCItem[] item = AddItem(itemNames, opcGroup);
            sw.Stop();
            ts = sw.Elapsed;
            Console.WriteLine("Add Item 完成，用时" + ts.TotalSeconds);
            sw.Reset();

            //单独开启一个线程完成获取值的转移和拆分
            Thread th = new Thread(InsertTask);
            th.Start();
            th.IsBackground = true;


            sw.Start();

            while (true)
            {
                Monitor.Enter(itemvalues);
                for (int i = 0; i < item.Length; i++)
                {
                    itemvalues[itemNames[i]] = Convert.ToString(item[i].Value);
                }
                Monitor.Exit(itemvalues);

                sw.Stop();
                ts = sw.Elapsed;
                Console.WriteLine("所有值获取完成，花费时间" + ts.TotalSeconds + "s");
                Thread.Sleep(800);
            }
        }


        private static OPCItem[] AddItem(List<string> item, OPCGroup opcGroup)   //把需要获取的标签点加到group中
        {
            OPCItem[] opcItem = new OPCItem[item.Count];
            for (int i = 0; i < item.Count; i++)
            {
                opcItem[i] = opcGroup.OPCItems.AddItem(item[i], 1);
            }
            return opcItem;
        }


        private static void InsertTask()
        {
            while (true)
            {
                int j = 45;
                Monitor.Enter(itemvalues);
                itemvaluesTemp = itemvalues;
                //工步变化
                for (int i = 0; i < 64; i++)
                {
                    //stepChangeState.Add(Convert.ToDouble(itemvaluesTemp[itemNames[j]]));
                    //j += 65;
                }
                //隔离阀状态变化

                //主加热变化
                //真空泵变化

                Monitor.Exit(itemvalues);
                Console.WriteLine("我是另一线程，我对采集数值进行了转移！");

                CompareValue();
                Thread.Sleep(1000);
            }
        }

        private static void InsertTaskOnTime()
        {
            while (true)
            {

                if (itemvaluesTemp != null)
                {
                    int m = 0, n = 65;
                    int k = 1, q = 0, p = 0;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    string[] sql = new string[64];
                    List<string> tableName = new List<string>();
                    for (int i = 93; i < 95; i++)
                    {
                        if (i < 9)
                        {
                            tableName.Add("YC_F0" + (i + 1).ToString());
                        }
                        else
                        {
                            tableName.Add("YC_F" + (i + 1).ToString());
                        }
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        sql[i] = "insert into " + tableName[i] + @"(" + @"nowtime,DIAMETER,SETDIAMETER,TEMP,MAINHEATER,SETMAINHEATER,
                         BOTTOMHEATER,SETBOTTOMHEATER,AVGSLSPEED,SETSLSPEED,MELTSURFTEMP,setmeltsurftemp,
                           meltlevel,setmeltlevel,seedlift,cruciblelift,seedrotation,cruciblerotation,
                            argonflow,mainpressure100t,mainpressure1000mt,subpressure1000t,diameterpixel,
                            meltlevelpixel,crystallength,cruciblepos,crystalpos,crystalweight,remainweight,
                            feederremainweight,feedervibration,feedersetargonflow,feederargonflow,pumpfrequency,
                            mainheatercurrent,bottomheatercurrent,runtime,feedquantity,unlimitedfeedquantity,
                            totalfeedquantity,meltlevelcoefficient,crownpixel,leakagerate,setpumpfrequency,
                            state,crystalid,mainpumpstatus,subpumpstatus,mainheaterstatus,bottomheaterstatus,
                            manualseed,isolationvalveopen,isolationvalveclose,mainpumpv1,subpumpv3,fastinflationv4,
                            upperargonv5,lowerargonv6,feedervalveopen,feedervalveclose,furnancecoverclose,
                            feederstate,feederpressure,crystalid6,ultimatevacuum,hotcheackedleakagerate,
                            receive_tag,receive_time,remark" + @")"
                            + " values( to_date('" + nowtime + "','yyyy-mm-dd hh24:mi:ss'),";

                        if (itemvaluesTemp[itemNames[(62*k)+p]] == null || itemvaluesTemp[itemNames[(62*k)+p]] == "")
                        {
                            itemvaluesTemp[itemNames[(62 *k)+p]] = "0";
                        }
                        for (int j = m; j < n; j++)
                        {
                            if (j==(44*k)+q)
                            {
                                sql[i] +="'"+ itemvaluesTemp[itemNames[j]]+"'" + ",";
                            }
                            else
                            {
                                sql[i] += itemvaluesTemp[itemNames[j]] + ",";
                            }
                            
                        }
                        string lastPoint1 = "0";
                        string lastPoint2 = nowtime;
                        string lastPoint3 = "0";
                        sql[i] += lastPoint1 + "," + "to_date('" + lastPoint2 + "','yyyy-mm-dd hh24:mi:ss')," + lastPoint3 + ")";
                        //SaveTextFile("1.txt", sql[i]);
                        try
                        {
                            string connString = "User ID=YCSCADA;Password=ycscada2019;Data Source=ORCL_YC;";
                            OracleConnection conn = new OracleConnection(connString);
                            conn.Open();
                            OracleCommand cmd = new OracleCommand(sql[i], conn);
                            cmd.ExecuteNonQuery();
                            n += 65; m += 65;k += 1;q += 21;p += 3;
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("连接Oracle出错" + ex.Message);
                        }
                    }
                    nowtime = Convert.ToDateTime(nowtime).AddSeconds(30).ToString();
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Console.WriteLine(">>>>>>>>>>>>单晶炉写入数据完成！用时>>>>>>>>" + ts.TotalSeconds + "s");
                    Thread.Sleep(30000);
                }

                else
                {
                    Console.WriteLine("++++++++我是定时保存线程，每30s执行一次,当前数据未刷新，我什么也没做！+++++");
                    Thread.Sleep(30000);
                }
            }
        }

        private static void CompareValue()
        {


            if (stepChangeState.Count != 0)
            {

                //int j = 0, m = 0, n = 64;
                List<string> tableName = new List<string>();
                //枚举出64台炉台存储表表名称
                for (int i = 0; i < 64; i++)
                {
                    if (i < 9)
                    {
                        tableName.Add("YC_F0" + (i + 1).ToString());
                    }
                    else
                    {
                        tableName.Add("YC_F" + (i + 1).ToString());
                    }
                }
                for (int i = 0; i < 64; i++)
                {

                    if (stepChangeStateCompare.Count != 0)      //值不相等
                    {
                        if (stepChangeStateCompare[i].CompareTo(stepChangeState[i]) == 0)
                        {


                            Console.WriteLine("-----我是状态变化监测线程，检测到" + tableName[i] + "step状态变化------");


                            //string sql= "insert into " + tableName[i] + " values( to_date('" + nowtime + "','yyyy-mm-dd hh24:mi:ss'),";
                            //for (j=m; j < n; j++)
                            //{
                            //    sql += itemvaluesTemp[itemnames[j]] + ",";
                            //}
                            //m += 64;n += 64;
                        }
                    }
                    else
                    {
                        stepChangeStateCompare = stepChangeState;
                    }
                }
            }


        }

        public static bool SaveTextFile(string path, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(936));
                    sw.Write(content);
                    sw.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("写入文件出错：消息={0},堆栈={1}", ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}