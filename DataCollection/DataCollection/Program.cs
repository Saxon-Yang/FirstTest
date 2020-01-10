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
        public static int firstCompare = 1;
        public static string nowtime = null;
        public static string tnowtime = null;
        public static string tag = "";
        public static Dictionary<string, string> itemvalues = new Dictionary<string, string>();
        public static Dictionary<string, string> itemvaluesTempOld = new Dictionary<string, string>();
        public static Dictionary<string, string> itemvaluesTemp;
        public static int count = 0;

        public static double[] stepChangeState = new double[96];//工步变化 STATE
        public static double[] stepChangeStateCompare = new double[96];

        public static double[] IsolationvalOpenChangeState = new double[96];//ISOLATIONVALVEOPEN  隔离阀
        public static double[] IsolationvalOpenChangeStateCompare = new double[96];

        public static double[] mainHeaterStatusChangeState = new double[96];//主加热变化  MAINHEATERSTATUS
        public static double[] mainHeaterStatusChangeStateCompare = new double[96];

        public static double[] mainPumpStatusChangeState = new double[96];//真空泵变化 MAINPUMPSTATUS
        public static double[] mainPumpStatusChangeStateCompare = new double[96];

        public static double[] functionState = new double[96];//真空泵变化 MAINPUMPSTATUS
        public static double[] functionStateCompare = new double[96];

        public static string[] stateTag1 = new string[96];
        public static string[] stateTag2 = new string[96];
        public static string[] stateTag3 = new string[96];
        public static string[] stateTag4 = new string[96];
        public static string[] stateTag5 = new string[96];

        public static string oraStr = "";
        public static List<string> itemNames = new List<string>();
        private static OPCServer opcServer = null;
        private static OPCGroup opcGroup = null;
        static void Main(string[] args)
        {

            //Thread th1 = new Thread(InsertTaskOnTime);
            //th1.Start();
            //th1.IsBackground = true;

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
            for (int i = 0; i < 96; i++)       //遍历炉台
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
                Thread.Sleep(1000);
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
                int j = 43;
                int k = 50;
                int m = 47;
                int n = 45;
                int o = 60;
                Monitor.Enter(itemvalues);
                itemvaluesTemp = itemvalues;
                //工步变化  43
                for (int i = 0; i < 96; i++)
                {
                    stepChangeState[i] = (Convert.ToDouble(itemvaluesTemp[itemNames[j]]));
                    j += 65;
                }
                //隔离阀状态变化  ISOLATIONVALVEOPEN 50
                for (int i = 0; i < 96; i++)
                {
                    IsolationvalOpenChangeState[i] = (Convert.ToDouble(itemvaluesTemp[itemNames[k]]));
                    k += 65;

                }
                //主加热变化  MAINHEATERSTATUS   47

                for (int i = 0; i < 96; i++)
                {
                    mainHeaterStatusChangeState[i] = (Convert.ToDouble(itemvaluesTemp[itemNames[m]]));
                    m += 65;
                }
                //真空泵变化 MAINPUMPSTATUS   45

                for (int i = 0; i < 96; i++)
                {
                    mainPumpStatusChangeState[i] = (Convert.ToDouble(itemvaluesTemp[itemNames[n]]));
                    n += 65;
                }
                //功能状态变化   60
                for (int i = 0; i < 96; i++)
                {
                    functionState[i] = (Convert.ToDouble(itemvaluesTemp[itemNames[o]]));
                    o += 65;
                }
                Console.WriteLine("++++++需要监测值改变的点已经分发完成++++++");
                Console.WriteLine("我是另一线程，我对采集数值进行了转移！");

                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (count / 30 == 1)
                {
                    nowtime = DateTime.Now.ToString();
                    CompareValue1();
                    InsertTaskOnTime();
                    count = 0;
                }
                else
                {
                    tnowtime = DateTime.Now.ToString();
                    CompareValue();
                    count++;
                }
                //nowtime = DateTime.Now.ToString();
                //CompareValue();
                //count++;
                //if (count==30)
                //{
                //    Thread.Sleep(300);
                //    CompareValue1();
                //    tnowtime = DateTime.Now.ToString();
                //    InsertTaskOnTime();
                //    count = 0;
                //}
                Monitor.Exit(itemvalues);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Console.WriteLine("++++++监测点值是否变化判断完成+++++++ 用时" + ts.TotalSeconds + "s");
                Thread.Sleep(1000);
            }
        }

        private static void InsertTaskOnTime()   //每30s插入一条数据
        {


            if (itemvaluesTemp != null)
            {
                int m = 0, n = 65;
                int k = 1, q = 0, p = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string[] sql = new string[96];
                List<string> tableName = new List<string>();
                for (int i = 0; i < 96; i++)
                {
                    if (i < 9)
                    {
                        tableName.Add("YC_I0" + (i + 1).ToString());
                    }
                    else
                    {
                        tableName.Add("YC_I" + (i + 1).ToString());
                    }
                }

                for (int i = 0; i < 96; i++)   //获取的点拆分成sql语句
                {
                    if (tableName[i].CompareTo(stateTag1[i]) != 0 && tableName[i].CompareTo(stateTag2[i]) != 0 && tableName[i].CompareTo(stateTag3[i]) != 0 && tableName[i].CompareTo(stateTag4[i]) != 0 && tableName[i].CompareTo(stateTag5[i]) != 0)
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

                        if (itemvaluesTemp[itemNames[(62 * k) + p]] == null || itemvaluesTemp[itemNames[(62 * k) + p]] == "")
                        {
                            itemvaluesTemp[itemNames[(62 * k) + p]] = "0";
                        }
                        for (int j = m; j < n; j++)
                        {
                            if (j == (44 * k) + q)
                            {
                                sql[i] += "'" + itemvaluesTemp[itemNames[j]] + "'" + ",";
                            }
                            else
                            {
                                sql[i] += itemvaluesTemp[itemNames[j]] + ",";
                            }
                        }
                        string lastPoint1 = "0";
                        string lastPoint2 = "null";
                        string lastPoint3 = "0";
                        sql[i] += lastPoint1 + "," + lastPoint2 + "," + lastPoint3 + ")";
                        OracleConnection conn = null;
                        try
                        {
                            string connString = "User ID=djzk_yc;Password=djzkapp01;Data Source=YCDJZK;";
                            conn = new OracleConnection(connString);
                            conn.Open();
                            OracleCommand cmd = new OracleCommand(sql[i], conn);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            n += 65; m += 65; k += 1; q += 21; p += 3;
                        }
                        catch (Exception ex)
                        {
                            n += 65; m += 65; k += 1; q += 21; p += 3;
                            conn.Close();
                            Console.WriteLine("连接Oracle出错" + ex.Message);
                        }
                    }
                    else
                    {
                        n += 65; m += 65; k += 1; q += 21; p += 3;
                    }
                }
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                Console.WriteLine(">>>>>>>>>>>>单晶炉写入数据完成！用时>>>>>>>>" + ts.TotalSeconds + "s");
                //Thread.Sleep(30000);
            }

            else
            {
                Console.WriteLine("++++++++我是定时保存线程，每30s执行一次,当前数据未刷新，我什么也没做！+++++");
                //Thread.Sleep(30000);
            }


        }

        private static void CompareValue()
        {

            if (firstCompare == 1)     //初始化的时候把第一次拿到的值作为比较标准值,只执行一次
            {
                for (int i = 0; i < 96; i++)
                {
                    stepChangeStateCompare[i] = stepChangeState[i];
                    IsolationvalOpenChangeStateCompare[i] = IsolationvalOpenChangeState[i];
                    mainHeaterStatusChangeStateCompare[i] = mainHeaterStatusChangeState[i];
                    mainPumpStatusChangeStateCompare[i] = mainPumpStatusChangeState[i];
                    functionStateCompare[i] = functionState[i];
                }
                firstCompare = 2;
            }
            string sql = "";
            int m = 0, n = 65, a = 0, a2 = 0, a3 = 0, a4 = 0;
            int k = 1, q = 0, p = 0;
            List<string> tableName = new List<string>();
            //枚举出64台炉台存储表表名称
            for (int i = 0; i < 96; i++)
            {
                if (i < 9)
                {
                    tableName.Add("YC_I0" + (i + 1).ToString());
                }
                else
                {
                    tableName.Add("YC_I" + (i + 1).ToString());
                }
            }
            for (int i = 0; i < 96; i++)
            {
                tag = "";
                if (Double.Equals(stepChangeStateCompare[i], stepChangeState[i]) == false || Double.Equals(IsolationvalOpenChangeStateCompare[i], IsolationvalOpenChangeState[i]) == false || Double.Equals(mainHeaterStatusChangeStateCompare[i], mainHeaterStatusChangeState[i]) == false || Double.Equals(mainPumpStatusChangeStateCompare[i], mainPumpStatusChangeState[i]) == false || Double.Equals(functionStateCompare[i], functionState[i]) == false)
                {

                    if (Double.Equals(stepChangeStateCompare[i], stepChangeState[i]) == false)
                    {
                        tag += "1";
                    }
                    if (Double.Equals(mainHeaterStatusChangeStateCompare[i], mainHeaterStatusChangeState[i]) == false)
                    {
                        tag += "2";
                    }
                    if (Double.Equals(mainPumpStatusChangeStateCompare[i], mainPumpStatusChangeState[i]) == false)
                    {
                        tag += "3";
                    }
                    if (Double.Equals(IsolationvalOpenChangeStateCompare[i], IsolationvalOpenChangeState[i]) == false)
                    {
                        tag += "4";
                    }
                    if (Double.Equals(functionStateCompare[i], functionState[i]) == false)
                    {
                        tag += "5";
                    }

                    sql = "insert into " + tableName[i] + @"(" + @"nowtime,DIAMETER,SETDIAMETER,TEMP,MAINHEATER,SETMAINHEATER,
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
                    + " values( to_date('" + tnowtime + "','yyyy-mm-dd hh24:mi:ss'),";

                    if (itemvaluesTemp[itemNames[(62 * k) + p]] == null || itemvaluesTemp[itemNames[(62 * k) + p]] == "")
                    {
                        itemvaluesTemp[itemNames[(62 * k) + p]] = "0";
                    }
                    for (int j = m; j < n; j++)
                    {

                        if (j == (44 * k) + q)
                        {
                            sql += "'" + itemvaluesTemp[itemNames[j]] + "'" + ",";
                        }
                        else if (j == (50 * k) + a2)
                        {
                            sql += IsolationvalOpenChangeState[i] + ",";
                        }
                        else
                        {
                            sql += itemvaluesTemp[itemNames[j]] + ",";
                        }

                    }
                    string lastPoint1 = "0";
                    string lastPoint2 = "null";
                    string lastPoint3 = tag;
                    sql += lastPoint1 + "," + lastPoint2 + "," + lastPoint3 + ")";
                    OracleConnection conn = null;
                    try
                    {
                        string connString = "User ID=djzk_yc;Password=djzkapp01;Data Source=YCDJZK;";
                        conn = new OracleConnection(connString);
                        conn.Open();
                        OracleCommand cmd = new OracleCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        n += 65; m += 65; k += 1; q += 21; p += 3; a += 22; a2 += 15; a3 += 18; a4 += 20;
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        n += 65; m += 65; k += 1; q += 21; p += 3; a += 22; a2 += 15; a3 += 18; a4 += 20;
                        Console.WriteLine("连接Oracle出错" + ex.Message);
                    }
                }

                else
                {
                    n += 65; m += 65; k += 1; q += 21; p += 3; a += 22; a2 += 15; a3 += 18; a4 += 20;
                    Console.WriteLine("未检测到隔离阀状态变化！");
                }
            }
            for (int i = 0; i < 96; i++)
            {
                stepChangeStateCompare[i] = stepChangeState[i];
                IsolationvalOpenChangeStateCompare[i] = IsolationvalOpenChangeState[i];
                mainHeaterStatusChangeStateCompare[i] = mainHeaterStatusChangeState[i];
                mainPumpStatusChangeStateCompare[i] = mainPumpStatusChangeState[i];
                functionStateCompare[i] = functionState[i];
            }  //改变比较的标准值
        }

        private static void CompareValue1()
        {
            for (int i = 0; i < 96; i++) //每一次比较清空标记
            {
                stateTag1[i] = "";
                stateTag2[i] = "";
                stateTag3[i] = "";
                stateTag4[i] = "";
                stateTag5[i] = "";
            }


            if (firstCompare == 1)     //初始化的时候把第一次拿到的值作为比较标准值,只执行一次
            {
                for (int i = 0; i < 96; i++)
                {
                    stepChangeStateCompare[i] = stepChangeState[i];
                    IsolationvalOpenChangeStateCompare[i] = IsolationvalOpenChangeState[i];
                    mainHeaterStatusChangeStateCompare[i] = mainHeaterStatusChangeState[i];
                    mainPumpStatusChangeStateCompare[i] = mainPumpStatusChangeState[i];
                    functionStateCompare[i] = functionState[i];
                }
                firstCompare = 2;
            }
            string sql = "";
            int m = 0, n = 65, a = 0, a2 = 0, a3 = 0, a4 = 0;
            int k = 1, q = 0, p = 0;
            List<string> tableName = new List<string>();
            //枚举出64台炉台存储表表名称
            for (int i = 0; i < 96; i++)
            {
                if (i < 9)
                {
                    tableName.Add("YC_I0" + (i + 1).ToString());
                }
                else
                {
                    tableName.Add("YC_I" + (i + 1).ToString());
                }
            }

            for (int i = 0; i < 96; i++)      //step值不相等
            {
                if (Double.Equals(stepChangeStateCompare[i], stepChangeState[i]) == false)
                {
                    stateTag1[i] = tableName[i];
                }

                else
                {
                    stateTag1[i] = "xxxxxx";
                }

            }
            m = 0; n = 65; k = 1; q = 0; p = 0; a = 0; a2 = 0; a3 = 0; a4 = 0;

            for (int i = 0; i < 96; i++)      //隔离阀值不相等
            {
                if (Double.Equals(IsolationvalOpenChangeStateCompare[i], IsolationvalOpenChangeState[i]) == false)
                {
                    stateTag2[i] = tableName[i];
                }

                else
                {
                    stateTag2[i] = "xxxx";
                }
            }

            for (int i = 0; i < 96; i++)      //主加值不相等
            {
                if (Double.Equals(mainHeaterStatusChangeStateCompare[i], mainHeaterStatusChangeState[i]) == false)
                {
                    stateTag3[i] = tableName[i];
                }

                else
                {

                    stateTag3[i] = "xxxx";
                }
            }


            for (int i = 0; i < 96; i++)      //主泵值不相等
            {
                if (Double.Equals(mainPumpStatusChangeStateCompare[i], mainPumpStatusChangeState[i]) == false)
                {
                    stateTag4[i] = tableName[i];
                }

                else
                {
                    stateTag4[i] = "xxxxx";
                }
            }


            for (int i = 0; i < 96; i++)      //功能状态值不相等
            {
                if (Double.Equals(functionStateCompare[i], functionState[i]) == false)
                {
                    stateTag5[i] = tableName[i];
                }
                else
                {
                    stateTag5[i] = "xxxxx";
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
                using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(content);
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