using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Struts2Checker
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
                Checker ochecker = new Checker();
                if (argdic.ContainsKey("-m"))
                {
                    if (argdic.ContainsKey("-f"))
                    {
                        if (argdic.ContainsKey("-t"))
                        {
                            //格式错误
                        }
                        else
                        {
                            List<string> tar_lst = Loader.LoadTargetFile(argdic["-f"]);
                            foreach (string tar in tar_lst)
                            {
                                if (Checker.Regexanalyize(tar, ".action?") != "")
                                {
                                    if (ochecker.CheckVul(tar, argdic["-m"])) Console.WriteLine("[+] 发现漏洞：" + tar);
                                    else Console.WriteLine("[+] 没有漏洞：" + tar);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ochecker.CheckVul(argdic["-t"], argdic["-m"])) Console.WriteLine("[+] 发现漏洞：" + argdic["-t"]);
                        else Console.WriteLine("[+] 没有漏洞：" + argdic["-t"]);
                    }
                }
                else
                {
                    PrintUsage();
                }
            }
            else
            {
                PrintUsage();
            }
            //Console.ReadKey();
        }

        public static void PrintUsage()
        {
            Console.WriteLine("[*]使用方法: schecker -t <url> -f <target file> -m <vul year>");
            Console.WriteLine("[*]year: 2012 - cve xxxxx");
            Console.WriteLine("[*]year: 2013 - s2-016");
        }
    }
}
