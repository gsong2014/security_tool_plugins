using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FingerCheck
{
    //web请求类
    class Webrequester
    {
        public static string url = "";
        public static WebHeaderCollection Header = new WebHeaderCollection();

        public static string Webrequest()
        {
            //返回数据
            string sData = "";
            
            int timeOut = 4000;
            url = url.Split('#')[0];
            //传入需要解析的网页地址
            if (Helper.IsURL(url))
            {
                WebRequest request = WebRequest.Create(url);
                //将标头设置为指定值
                request.Headers.Set("pragma", "no-cache");
                WebResponse response = null;
                try
                {
                    //可能会有超时异常
                    request.Timeout = timeOut;
                    response = request.GetResponse();//获取URL数据
                    Header = response.Headers;
                }
                catch (WebException e)
                {
                    Console.WriteLine("[*] " + e.Message);
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                sData = reader.ReadToEnd();

                return sData;
            }
            else
                Console.WriteLine("[*] 错误的地址格式: "+url);
                return "";
        }
    }
}
