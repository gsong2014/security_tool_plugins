
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace FingerCheck
{
    
    //指纹库解析类
    class Parser
    {
        //指纹库路径
        private static string appsFile = Directory.GetCurrentDirectory() + "\\apps_finger.json";

        //加载指纹库
        public static string LoadFingers()
        {
            try
            {
                
                FileStream sFile = new FileStream(appsFile, FileMode.Open);
                StreamReader m = new StreamReader(sFile);
                string str = "";
                while (!m.EndOfStream)
                    str += m.ReadLine();
                return str;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[*] 指纹文件未找到，请放置在应用程序目录下.");
                return null;
            }
        }

        //反序列化
        public static App[] Deserialize()
        {
            //反序列化json到字典
            string fingers = "";
            try
            {
                fingers = LoadFingers();
            }
            catch
            {
            }
            if (fingers != null)
            {
                JavaScriptSerializer s = new JavaScriptSerializer();
                Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(fingers);
                Dictionary<string, object> categories = (Dictionary<string, object>)JsonData["categories"];
                Dictionary<string, object> apps = (Dictionary<string, object>)JsonData["apps"];

                Dictionary<string, string> cat = new Dictionary<string, string>();
                foreach (KeyValuePair<string, object> ss in categories)
                {
                    cat.Add(ss.Key.ToString(), ss.Value.ToString());
                }

                //建立app对象，独立存放属性
                App[] app = new App[apps.Count];
                int i = 0;

                //解析到app对象
                foreach (KeyValuePair<string, object> so in apps)
                {
                    app[i] = new App();
                    app[i].Appname = so.Key;
                    app[i].attr = (Dictionary<string, object>)apps[so.Key];

                    //获取应用的分类
                    object[] cats = (object[])app[i].attr["cats"];
                    foreach (object ob in cats)
                    {
                        app[i].Cats.Add(cat[ob.ToString()]);
                    }
                    i++;
                }
                return app;
            }
            else
                return null;
        }
    }
}
