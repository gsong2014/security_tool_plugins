using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PathSeeker
{
    class Checker
    {
        public static void ThreadCheck()
        {
            
            string tar = GetMission();
            if (tar != null)
            {
                Console.WriteLine("[-] 正在检查："+tar);
                //声明Checker对象，负责检测目标返回的头信息
                Webrequester req = new Webrequester();
                //获取状态码
                string tmptar = tar;
                if (!tmptar.StartsWith("http://")) tmptar = "http://" + tmptar;
                req.CheckUri(tmptar);
                //Console.WriteLine("[-] 检查完成：" + tar);
                global.Finished += 1;
            }
            else
                Console.WriteLine("领取任务超出列表范围~~~");
        }

        public static void LoadCGI(string url,string dic)
        {
            //逐行读取
            try
            {
                using (StreamReader sr = new StreamReader(dic))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        global.cgilist.Add(url + line);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("[*]"+err.Message + " @loadfuzz");
            }
            //读取结束
        }

        private static string GetMission()
        {
            if (global.cgilist.Count > 0)
            {
                string tmpItem = global.cgilist[0];
                global.cgilist.RemoveAt(0);
                return tmpItem;
            }
            else
                return null;
        }
    }
}
