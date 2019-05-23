using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathSeeker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length % 2 == 0)
            {
                Dictionary<string, string> argdic = new Dictionary<string, string>();
                for (int i = 0; i < args.Length - 1; i += 2)
                {
                    argdic.Add(args[i], args[i + 1]);
                }
                if (argdic.ContainsKey("-t") && argdic.ContainsKey("-d"))
                {
                    Console.WriteLine("[-] 目标：" + argdic["-t"]);
                    Console.WriteLine("[-] 正在读取字典：" + argdic["-d"]);
                    //读取CGI列表
                    Checker.LoadCGI(argdic["-t"], argdic["-d"]);
                    //创建线程
                    Console.WriteLine("[-] 正在创建线程池...");
                    global.mythread = new Thread[global.cgilist.Count];

                    while (global.threadID < global.cgilist.Count)
                    {
                        global.mythread[global.threadID] = new Thread(new ThreadStart(Checker.ThreadCheck));
                        //初始化一个线程实例
                        global.threadID++;
                    }
                    global.threadID = 0;
                    Console.WriteLine("[-] 检查中...");
                    //开始检查
                    while (true)
                    {
                        global.gRunningThreadCount = 0;
                        //检查当前正在运行的线程
                        foreach (Thread countThread in global.mythread)
                        {
                            if (countThread != null)
                            {
                                if (countThread.ThreadState == ThreadState.Running)
                                    global.gRunningThreadCount++;
                            }
                        }

                        //未达到最大线程，开启新线程
                        while (global.gRunningThreadCount < global.gMaxThreadCount && global.threadID < global.mythread.Length)
                        {
                            //启动线程
                            global.mythread[global.threadID].Start();
                            global.gRunningThreadCount++;
                            global.threadID++;
                        }
                        //Console.Clear();
                        //Console.WriteLine("[-] 剩余：" + global.cgilist.Count.ToString() + "\t已发现：" + global.ulist.Count);
                        
                        //查询是否所有线程停止
                        //Console.WriteLine("[-] 线程数：" + global.gRunningThreadCount);

                        if (global.Finished == global.mythread.Length)
                            break;
                    }
                    //Console.Clear();
                    foreach (KeyValuePair<string, int> result in global.ulist)
                    {
                        Console.WriteLine("[+] " + result.Value + "\t" + result.Key);
                    }

                    foreach (KeyValuePair<string, int> result in global.wlist)
                    {
                        if(result.Value!= 404)
                            Console.WriteLine("[+] " + result.Value + "\t" + result.Key);
                    }
                    if (global.ulist.Count == 0 && global.wlist.Count == 0)
                        Console.WriteLine("[+] 未发现符合条件的目标");
                    Console.WriteLine("[-] 任务完成");
                }
                else
                    PrintUsage();
            }
            else
                PrintUsage();
            //Console.ReadKey();
        }
        public static void PrintUsage()
        {
            Console.WriteLine("[*] 使用方法：PathSeeker -t www.baidu.com -d cgi.list");
        }
    }

    public static class global
    {
        public static Dictionary<string,int> ulist = new Dictionary<string,int>();
        public static Dictionary<string, int> wlist = new Dictionary<string, int>();
        public static List<string> cgilist = new List<string>();
        public static string savePath = @"C:\";
        public static int gMaxThreadCount = 25;
        public static int gRunningThreadCount = 0;
        public static int gTimeout = 6000;
        public static string gRequestType = "GET";
        public static string[] portlist = { "80", "8080" };
        public static Thread[] mythread;
        public static int threadID = 0;
        public static bool SaveResult = true;
        public static int Finished = 0;
    }
}
