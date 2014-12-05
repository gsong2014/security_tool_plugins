//-----------------------------------------------------------------------
// <copyright file="UrlHelper.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.HelpFunction
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using EasySpider.Model;

    class FileHelper
    {
        /// <summary>
        /// create a file ,and write down result
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="linkInfoDictionary">LinkInfo dictionary</param>
        public static void CreateResultFile(string path, Dictionary<string,LinkInfo> linkInfoDictionary)
        {
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Close();
            }
            else 
            {
                File.Delete(path);
                StreamWriter sw = File.CreateText(path);
                sw.Close();
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                if (linkInfoDictionary != null)
                    {
                        foreach (var li in linkInfoDictionary)
                        { 
                            LogResult(li.Key,sw);
                        }
                    }              
            }
        }

        /// <summary>
        /// log function
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="sw">StreamWriter</param>
        private static void LogResult(string message, StreamWriter sw)
        {
            sw.WriteLine(message);
        }
    }
}
