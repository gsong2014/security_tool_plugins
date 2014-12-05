using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Struts2Checker
{
    //正则匹配类
    class Checker
    {
        public bool DiffTime(int sOriginal, int sAlter)
        {
            //比较两次结果之差
            int diff = sAlter - sOriginal;
            Console.WriteLine("[-] Diff:"+diff);
            if (diff > 5000)
            {
                return true;
            }
            else
                return false;
        }

        public bool CheckVul(string Target,string Method)
        {
            Console.WriteLine("[-] 检测目标:" + Target);
            if (!Target.StartsWith("http://"))
                Target = "http://" + Target;
            Console.WriteLine("[-] 使用漏洞:" + Method);
            if (Method == "2012")
            {
                int sResponseOriginal = 0;
                int sResponseVulable = 0;
                Webrequester oOriWebRequester = new Webrequester();
                Webrequester oVulWebRequester = new Webrequester();
                try
                {
                    Uri url = new Uri(Target);
                    //正常请求
                    sResponseOriginal = oOriWebRequester.ResponseTime(Target, "Original");
                    Console.WriteLine("[-] 原始请求:" + sResponseOriginal.ToString());

                    //测试请求
                    sResponseVulable = oVulWebRequester.ResponseTime(Target, "Vul");
                    Console.WriteLine("[-] 检测请求:" + sResponseVulable.ToString());
                }
                catch (UriFormatException)
                {
                    Console.WriteLine("[*] URL格式错误！");
                }

                //通过响应时间判断是否有漏洞
                if (DiffTime(sResponseOriginal, sResponseVulable)) return true;
                else return false;
            }
            if (Method == "2013")
            {
                string data = "";
                Webrequester oVulWebRequester = new Webrequester();
                try
                {
                    Uri url = new Uri(Target);
                    //正常请求
                    data = oVulWebRequester.Webrequest(Target, "2013");
                    //Console.WriteLine(data);
                    if (Regexanalyize(data, "dolo") != "") return true;
                    else return false;
                }
                catch (UriFormatException ufe)
                {
                    Console.WriteLine("[*] URL格式错误:"+ ufe.Message);
                }
                return false;
            }
            else return false;
            
        }

        public static string Regexanalyize(string data, string re)
        {
            Regex regex = new Regex(re);
            Match m = regex.Match(data);
            return m.Value;
        }
    }
}
