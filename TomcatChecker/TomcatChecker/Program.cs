using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace TomcatChecker
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
                {
                    Console.WriteLine("[-] 正在加载列表...");
                    TargetList = LoadFile(argdic["-f"]);
                }
                else if (argdic.ContainsKey("-t"))
                {
                    TargetList.Clear();
                    TargetList.Add(argdic["-t"]);
                }

                foreach (string readT in TargetList)
                {
                    Console.WriteLine("[-] 目标：" + readT);
                    string url = readT + "/manager/html";
                    if (!url.StartsWith("http://"))
                        url = "http://" + url;
                    try
                    {
                        if (CheckVul(url, "", "") == HttpStatusCode.Unauthorized)//检查是否需要鉴权
                        {
                            Console.WriteLine("[-] 发现tomcat后台：" + url);
                            if (CheckVul(url, "tomcat", "tomcat") == HttpStatusCode.OK)//检查用户名密码对是否正确
                                Console.WriteLine("[+] 发现弱口令tomcat/tomcat");
                            else//密码错误的情况
                                Console.WriteLine("[-] 密码错误");
                        }
                        else//不需要鉴权的页面
                        {
                            Console.WriteLine("[+] 未发现tomcat后台");
                        }
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("[*] " + ex.Message);
                    }
                }
            }
            Console.WriteLine("[-] 检查结束");
            Console.ReadKey();
        }

        static HttpStatusCode CheckVul(string url,string username,string password)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //格式为 "username:password"
            string usernamePassword = username + ":" + password;
            //构造凭据存储空间
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
            webRequest.Credentials = mycache;
            webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
            try
            {
                //发出请求
                HttpWebResponse res = webRequest.GetResponse() as HttpWebResponse;
                return res.StatusCode;
            }
            catch (WebException wex)
            {
                HttpWebResponse response = (HttpWebResponse)wex.Response;
                if (response != null)//没有response的情况（通常是未开放端口）
                    return response.StatusCode;
                else
                    return HttpStatusCode.BadRequest;//随便返回一个
            }
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
            catch (Exception ex)
            {
                Console.WriteLine("[*] " + ex.Message);
                ShowUsage();
                return false;
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("[*]使用方法: TomcatChecker -t http://baidu.com");
            Console.WriteLine("[*]使用方法: TomcatChecker -f tar_list.txt");
        }
    }
}
