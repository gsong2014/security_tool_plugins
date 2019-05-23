//-----------------------------------------------------------------------
// <copyright file="Excute.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EasySpider.HelpFunction;
    using EasySpider.Model;
    using EasySpider.Operation;

    class Excute
    {/// <summary>
        /// site, test environment
        /// </summary>
        public static string Site = "";

        /// <summary>
        /// XPath, target
        /// </summary>
        public static string Target = "";

        /// <summary>
        /// String, Word
        /// </summary>
        public static string Word = "";

        /// <summary>
        /// output file name, a excel
        /// </summary>
        public static string OutputFileName = "";

        /// <summary>
        /// grab numbers of webpages as your setting
        /// </summary>
        public static int Grab = 50000;

        /// <summary>
        /// How many layer of webpage will be scanned?
        /// </summary>
        public static int Layer = 2;

        static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("EasySpider.exe -s(网址) www.10086.cn/bj -l(层) 3 -f(文件) 文件名");
                Console.WriteLine("OR");               
                Console.WriteLine("Fox example:");
                Console.WriteLine("EasySpider.exe -s www.xxt.cn");
                Console.WriteLine("EasySpider.exe -s www.xxt.cn -L 1 -f result");               
                return;
            }
            ///get site address
            Site = GetSite(args);            
            if (String.IsNullOrEmpty(Site))
            {                
                Console.WriteLine("错误的网站");                
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("开始运行...");
            
            ///get layer
            Layer = GetLayer(args);
            Dictionary<string, LinkInfo> linkInfoDictionary = Spider.GetTotalLinkSet();
            foreach (var it in linkInfoDictionary)
            {
                Console.WriteLine("[+]{0}", it.Key);
            }
            Console.WriteLine("发现超链接：{0} 个",linkInfoDictionary.Count) ;
            ///get file name
            OutputFileName = GetOutputFileName(args);
            if (!string.IsNullOrEmpty(OutputFileName))
            {                    
                Console.WriteLine("开始生成结果文件");
                string filename = Environment.CurrentDirectory + @"\" + OutputFileName.Replace(".txt","") + ".txt";
                FileHelper.CreateResultFile(filename, linkInfoDictionary);
                Console.WriteLine("文件生成!");
                Console.WriteLine(filename);
                Console.ResetColor();
            }
#if DEBUG
            Console.Read();
#endif
            return;            
        }

        /// <summary>
        /// Get site from arguments list
        /// </summary>
        /// <param name="args">arguments list</param>
        /// <returns>site, test environment</returns>
        static string GetSite(string[] args)
        {           
            if (args == null || args.Length < 2)
                return null;

            string site = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Compare(args[i], "-s", true) == 0)
                {
                    if (++i < args.Length)
                        site = args[i];
                    break;
                }
            }
            if (!site.StartsWith("http:") && !site.StartsWith("https:"))
            {
                site = @"http://" + site;
            }
            Uri u = null;
            try
            {
                u = new Uri(site);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error040:" + e.Message);
                return null;
            }
            return site;
        }

        /// <summary>
        /// Get target from arguments list
        /// </summary>
        /// <param name="args">arguments list</param>
        /// <returns>target, XPath</returns>
        static string GetTarget(string[] args)
        {
            if (args == null || args.Length < 2)
                return null;

            string target = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Compare(args[i], "-t", true) == 0)
                {
                    if (++i < args.Length)
                        target = args[i];
                    break;
                }
            }
            return target;
        }

        /// <summary>
        /// Get Special word from arguments list
        /// </summary>
        /// <param name="args">arguments list</param>
        /// <returns>word, string</returns>
        static string GetParticularWord(string[] args)
        {
            if (args == null || args.Length < 2)
                return null;

            string word = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Compare(args[i], "-w", true) == 0)
                {
                    if (++i < args.Length)
                        word = args[i];
                    break;
                }
            }
            return word;
        }

        /// <summary>
        /// Get output file name from arguments list
        /// </summary>
        /// <param name="args">arguments list</param>
        /// <returns>Output file name</returns>
        static string GetOutputFileName(string[] args)
        {
            if (args == null || args.Length < 2)
                return null;

            string outputfilename = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Compare(args[i], "-f", true) == 0)
                {
                    if (++i < args.Length)
                        outputfilename = args[i];
                    break;
                }
            }
            return outputfilename;
        }

        /// <summary>
        /// Get layer from arguments list
        /// </summary>
        /// <param name="args">arguments list</param>
        /// <returns>layer</returns>
        private static int GetLayer(string[] args)
        {
            if (args == null || args.Length < 2)
                return Excute.Layer;

            int layer = -1;
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Compare(args[i], "-l", true) == 0)
                {
                    if (++i < args.Length)
                        layer = int.Parse(args[i]);
                    break;
                }
            }
            if (layer > 0 && layer < 99)
            {
                return layer;
            }
            else
            {
                return Excute.Layer;
            }
        }
    }
}
