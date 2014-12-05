using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;


namespace FTPAuditor
{
    class Program
    {
        static Dictionary<string, string> argdic = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            
            if (ParseArgs(args))
            {
                List<string> TargetList = new List<string>();//目标列表
                if (argdic.ContainsKey("-f"))
                    TargetList = LoadFile(argdic["-f"]);
                else if (argdic.ContainsKey("-t"))
                {
                    TargetList.Clear();
                    TargetList.Add(argdic["-t"]);
                }

                foreach (string readT in TargetList)
                {
                    Console.WriteLine("[-] " + readT + " 审计中.....");
                    foreach (string item_un in LoadFile("us.txt"))
                    {
                        foreach (string item_pw in LoadFile("pw.txt"))
                        {
                            try
                            {
                                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + readT);
                                request.Method = WebRequestMethods.Ftp.ListDirectory;
                                request.Credentials = new NetworkCredential(item_un, item_pw);
                                request.GetResponse();
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("[+] 找到密码： " + item_un + "/" + item_pw);
                                request.Abort();
                                Console.Beep(1000, 500);
                                break;
                            }
                            catch (Exception te)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("[-] 尝试失败: " + te.Message + item_un + "/" + item_pw);
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                        }
                    }
                }
            }
            else
                ShowUsage();
            //Console.ReadKey();
        }

        static List<string> LoadFile(string fName)
        {
            List<string> ItemList = new List<string>();
            string lines;
            System.IO.StreamReader file = new System.IO.StreamReader(Environment.CurrentDirectory + "\\" + fName);
            while ((lines = file.ReadLine()) != null)
            {
                try
                {
                    ItemList.Add(lines);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[*] " + ex.Message);
                }
            }
            file.Close();
            return ItemList;
        }

        private static bool ParseArgs(String[] args)
        {
            try
            {
                if (args.Length > 0 && args.Length % 2 == 0)
                {

                    for (int i = 0; i < args.Length - 1; i += 2)
                    {
                        argdic.Add(args[i], args[i + 1]);
                    }
                    return true;
                }
                else
                {
                    ShowUsage();
                    return false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("[*] " + ex.Message);
                ShowUsage();
                return false;
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("[*]使用方法: FTPAuditor -t 123.123.123.123");
            Console.WriteLine("[*]使用方法: FTPAuditor -f tar_list.txt");
        }
    }
}
