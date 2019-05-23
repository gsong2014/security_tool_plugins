using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Struts2Checker
{
    class Loader
    {
        public static List<string> LoadTargetFile(string filepath)
        {
            List<string> tar_lst = new List<string>();
            //逐行读取
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        tar_lst.Add(line);
                    }
                }
                return tar_lst;
            }
            catch (Exception err)
            {
                Console.WriteLine("[*] "+err.Message + " @loadfile");
                return null;
            }
            //读取结束
        }
    }
}
