using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace PathSeeker
{
    //web请求类
    class Webrequester
    {
        public int CheckUri(string uri)
        {
            int sCode = 0;//返回的状态码
            if (ValidateDateUrl(uri.TrimEnd('/')))
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                    req.Method = global.gRequestType;
                    req.Timeout = global.gTimeout;
                    req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.1.4) Gecko/20091016 Firefox/3.5.4 (.NET CLR 3.5.30729)";
                    //req.Timeout = 8000;
                    try
                    {
                        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                        sCode = (int)res.StatusCode;
                        global.ulist.Add(uri,sCode);
                        //Console.WriteLine(uri + " : " + sCode);
                        return sCode;
                        
                    }
                    //异常部分编码
                    catch (WebException wex)
                    {
                        HttpWebResponse response = (HttpWebResponse)wex.Response;
                        //有些服务器的安全设置导致返回的不是标准的http status code，而是“服务器提交了协议冲突. Section=ResponseStatusLine"”
                        if (response != null)
                        {
                            sCode = (int)response.StatusCode;
                            //Console.WriteLine(uri + " : " + sCode);
                            //if (response.StatusCode ==HttpWebResponseStatusCode.InternalServerError)
                            try
                            {
                                global.wlist.Add(uri, sCode);
                            }
                            catch (ArgumentException aex)
                            {
                                System.Diagnostics.Debug.WriteLine("[*] " + aex.Message);
                            }
                            return sCode;
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("[*] " + wex.Message);
                    }
                }
                catch (UriFormatException)
                {

                }
            }
            else { Console.WriteLine("[*] 非法的url：" + uri); };
            return 0;
        }

        public bool ValidateDateUrl(string input)
        {
            return Regex.IsMatch(input, "http(s)?://([\\w-]+\\.)+[\\w-]+(//[\\w- .//?%&=]*)?");
        }
    }


}
