//-----------------------------------------------------------------------
// <copyright file="Spider.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Operation
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using EasySpider.Model;

    /// <summary>
    /// provide methods to get and sync urls.
    /// </summary>
    public class Spider
    {
        /// <summary>
        /// get total links for Tag website
        /// </summary> 
        /// <returns>return a url dictionary which is gotten by spider method</returns>       
        public static Dictionary<string, LinkInfo> GetTotalLinkSet()
        {
            string homePage = Excute.Site;
            return GetLinkUnderDefinitePath(homePage, (new Uri(homePage)).Host);
        }           
        
        /// <summary>
        /// New: get total links for special root
        /// </summary>        
        /// <param name="rootPage">It is the root, or is the first page</param>
        /// <param name="host">host address</param>
        /// <returns>return a link info dictionary under under host</returns>
        private static Dictionary<string, LinkInfo> GetLinkUnderDefinitePath(string rootPage, string host)
        {
            Dictionary<string, LinkInfo> returnUrls = new Dictionary<string, LinkInfo>();
            Dictionary<string, LinkInfo> temp = new Dictionary<string, LinkInfo>();

            temp.Add(rootPage, null);
            bool keepRunning = false;
            int layer = 0;
            while (temp != null && temp.Count > 0 && !keepRunning)
            {
                foreach (var keyValue in temp)
                {
                    returnUrls.Add(keyValue.Key, keyValue.Value);
                }
                if(keepRunning = (returnUrls.Count > Excute.Grab || layer >= Excute.Layer))
                {
                    break;
                }
                temp = GetUrls(temp, host);
                layer++;
                foreach (var keyValue in returnUrls)
                {
                    temp.Remove(keyValue.Key);
                }                
            }
            return returnUrls;
        }
                
        /// <summary>
        /// Gets all LinkInfo of every url from input url dictionary
        /// </summary>        
        /// <param name="inputURL">input url dictionary</param>
        /// <param name="host">host of thiw websit</param>        
        /// <returns>get a urls dictionary which doesn't contain the urls in input url dictionary</returns>        
        private static Dictionary<string, LinkInfo> GetUrls(Dictionary<string, LinkInfo> inputDictionary, string host)
        {
            if (inputDictionary == null || inputDictionary.Count < 1)
            {
                return null;
            }

            var containers = from it in inputDictionary
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount * 5)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                             select Page.Get(it.Key);

            Dictionary<string, LinkInfo> outputDictionary = new Dictionary<string, LinkInfo>();
            string targetUrl;
            foreach (var pageInfo in containers)
            {
                if (pageInfo != null && pageInfo.LinkSet != null)
                {
                    int i = -1;
                    foreach (LinkInfo s in pageInfo.LinkSet.LinkList)
                    {                        
                        i++;
                        targetUrl = s.LinkUrl.Data;
                        Console.WriteLine("处理 {0}",targetUrl);
                        Uri u = null;
                        try
                        {
                            u = new Uri(targetUrl);
                        }
                        catch(Exception e)
                        {
#if DEBUG
                            Console.WriteLine("Error099:"+ e.Message);
#endif
                            break;
                        }
                        if (u.Host.Contains( host.Remove(0,host.IndexOf('.')+1)))
                        {
                            if (!targetUrl.Contains(".zip") && !targetUrl.Contains(".pdf") && !targetUrl.Contains(".rar")
                                && !targetUrl.Contains("mailto:") && !targetUrl.Contains("#")
                                && !targetUrl.Contains("errorpage.aspx?aspxerrorpath") && !targetUrl.Contains("returnurl=")
                                && !targetUrl.Contains(@".aspx?filter="))
                            {
                                try
                                {
                                    if (!inputDictionary.ContainsKey(targetUrl))
                                    {
                                        if (!outputDictionary.ContainsKey(targetUrl))
                                        {
                                            Console.WriteLine("发现链接 {0}", targetUrl);
                                            outputDictionary.Add(targetUrl, s);
                                        }
                                    }
                                }
                                catch (ArgumentException)
                                {
#if DEBUG
                                    Console.WriteLine("Error101: An element with the same key already exists in the Dictionary.");
#endif
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error100:" + e.Message);
                                }
                            }
                        }
                    }
                }
            }

            return outputDictionary;
        }
                
        /// <summary>
        /// remove duplicate LinkInfo
        /// </summary>
        /// <param name="linkSet">LinkInfo list</param>
        /// <returns>the new list without duplicate LinkInfo</returns>
        private static IList<LinkInfo> RemoveDuplicateLink(IList<LinkInfo> linkSet)
        {
            IList<LinkInfo> resultLinkSet = new List<LinkInfo>();
            foreach (LinkInfo link in linkSet)
            {
                if (!resultLinkSet.Contains(link))
                {
                    resultLinkSet.Add(link);
                }
            }

            return resultLinkSet;
        }
    }
}
