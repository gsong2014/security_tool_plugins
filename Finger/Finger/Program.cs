using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FingerCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("[*] 用法错误！用法：fingercheck http://www.baidu.com");
            }
            else
            {
                string UnformattedURI = args[0];
                if (!UnformattedURI.StartsWith("http://"))
                    UnformattedURI = "http://" + UnformattedURI;

                if (Helper.IsURL(UnformattedURI))
                {
                    Webrequester.url = UnformattedURI;
                    try
                    {
                        Console.WriteLine("[-] 目标:" + Webrequester.url);
                        Checker oChecker = new Checker();
                        oChecker.Check(Parser.Deserialize(), Webrequester.Webrequest());
                        oChecker.ProceedResult();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[*] " + e.Message);
                    }
                    Console.WriteLine("[-] 任务完成");
                }
                else
                {
                    Console.WriteLine("[*] URL格式错误！");
                }
                
            }
            //Console.ReadKey();
        }

        //static string TestFile()
        //{
        //    FileStream sFile = new FileStream("p.txt", FileMode.Open);
        //    StreamReader m = new StreamReader(sFile);
        //    string str = "";
        //    while (!m.EndOfStream)
        //    {
        //        str += m.ReadLine();
        //        str += "\n";
        //    }
        //    return str;
        //}

    }
}
