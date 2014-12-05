using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FingerCheck
{
    //正则匹配类
    class Checker
    {
        //识别出的app列表以及imply的容器及语言
        private List<App> DetectedAPP = new List<App>();

        ////输出结果
        public void ProceedResult()
        {
            //列表去重
            DetectedAPP = DetectedAPP.Distinct().ToList();

            //输出结果
            foreach (App res in DetectedAPP)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string c in res.Cats)
                {
                    sb.Append(c);
                }
                Console.Write("[+] " + res.Appname );
                Console.Write("\t 类型: " + sb.ToString());
                try
                {
                    Console.Write("\t 实现: " + res.attr["implies"].ToString());
                }
                catch (KeyNotFoundException e)
                {
                    //Console.WriteLine("[*] " + e.Message);
                }
                Console.Write("\t 官方网站: " + res.attr["website"].ToString());
                Console.WriteLine();
            }
        }

        //指纹检查
        public void Check(App[] apps, string data)
        {
            if (apps != null)
            {
                foreach (App app in apps)
                {
                    bool Detected = false;
                    foreach (KeyValuePair<string, object> kp in app.attr)
                    {
                        switch (kp.Key)
                        {
                            case "url":
                                //check url
                                if (Regexanalyize(Webrequester.url, kp.Value.ToString()))
                                {
                                    //确认html符合特征
                                    Detected = true;
                                }
                                break;
                            case "html":
                                //check html
                                if(kp.Value is string)
                                {
                                    if (Regexanalyize(data, kp.Value.ToString()))
                                    {
                                        //确认html符合特征
                                        Detected = true;
                                    }
                                }
                                else if (kp.Value is object[])//多特征的
                                {
                                    object[] tmpObj = (object[])kp.Value;
                                    foreach (object _htmlReg in tmpObj)
                                    {
                                        if (Regexanalyize(data, _htmlReg.ToString()))
                                        {
                                            //确认html符合特征
                                            Detected = true;
                                        }
                                    }
                                }
                                break;
                            case "script":
                                //check script
                                if (kp.Value is string)
                                {
                                    if (Regexanalyize(data, "<script[^>]+src=(\"|\')([^\"\']+)"))
                                    {
                                        foreach (string cp in Regcaps(data, "<script[^>]+src=(\"|\')([^\"\']+)"))
                                        {
                                            if (Regexanalyize(cp, kp.Value.ToString().Replace("\\;version:\\1", "")))
                                            {
                                                //确认script符合特征
                                                Detected = true;
                                            }
                                        }
                                    }
                                }
                                else if (kp.Value is object[])//多特征的
                                {
                                    object[] tmpObj = (object[])kp.Value;
                                    foreach (object _scriptReg in tmpObj)
                                    {
                                        string[] scripts_in_data = Regcaps(data, "<script[^>]+src=(\"|\')([^\"\']+)");
                                        foreach (string cp in scripts_in_data)
                                        {
                                            string _script_value = _scriptReg.ToString().Replace("\\;version:\\1", "");
                                            //if (app.Appname == "jQuery")
                                            //{
                                            //    Console.WriteLine(scripts_in_data.Length);
                                            //    Console.WriteLine("data:"+cp+" "+ _script_value);
                                            //}
                                            if (Regexanalyize(cp, _script_value))
                                            {
                                                //确认script符合特征
                                                Detected = true;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "meta":
                                //check meta
                                if (kp.Value is object)
                                {
                                    //Console.WriteLine(app.Appname+" is ob");
                                    Dictionary<string, object> meta_dic = (Dictionary<string, object>)kp.Value;
                                    foreach (KeyValuePair<string, object> _meta in meta_dic)
                                    {
                                        string _header_value = _meta.Value.ToString();
                                        if (Regexanalyize(Webrequester.Header[_meta.Key], _header_value))
                                        {
                                            //确认header符合特征
                                            Detected = true;
                                        }
                                    }
                                }
                                break;
                            case "headers":
                                //check headers
                                Dictionary<string, object> header_dic = (Dictionary<string, object>)kp.Value;
                                foreach (KeyValuePair<string, object> _header in header_dic)
                                {
                                    string _header_value = _header.Value.ToString().Replace("\\;version:\\1", "");
                                    if (Regexanalyize(Webrequester.Header[_header.Key], _header_value))
                                    {
                                        //确认header符合特征
                                        Detected = true;
                                    }
                                }
                                break;
                            case "env":
                                //check env
                                if (kp.Value is string)
                                {
                                    if (Regexanalyize(data, kp.Value.ToString().Replace("^","").Replace("$","")))
                                    {
                                        //确认html符合特征
                                        Detected = true;
                                    }
                                }
                                break;
                            case "implies":
                                //实现方法，可能输出多个重复对象
                                if (Detected)
                                {
                                    foreach (App BaseApp in apps)
                                    {
                                        if (BaseApp.Appname == kp.Value.ToString())
                                        {
                                            DetectedAPP.Add(BaseApp);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    if (Detected) DetectedAPP.Add(app);
                }
            }
        }

        //正则匹配类，返回是否成功匹配
        private static bool Regexanalyize(string data, string re)
        {
            if (data != null && re != null)
            {
                Regex regex = new Regex(re);
                Match m = regex.Match(data);
                return m.Success;
            }
            else
                return false;
        }

        //正则匹配类，返回匹配项
        private static string[] Regcaps(string data, string re)
        {
            if (data != null && re != null)
            {
                Regex regex = new Regex(re);
                MatchCollection mc = Regex.Matches(data, re); //满足pattern的匹配集合
                string[] caps = new string[mc.Count];
                int i = 0;
                foreach (Match match in mc)
                {
                    caps[i] = match.ToString();
                    i++;
                }
                return caps;
            }
            else
                return null;
        }
    }
}
