//-----------------------------------------------------------------------
// <copyright file="Page.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Operation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using HtmlAgilityPack;
    using EasySpider.Model;

    /// <summary>
    /// Methods to access web
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Get page information according to the url.
        /// </summary>
        /// <param name="url">request url</param>
        /// <returns>get a instance of PageResponseInfo</returns>        
        public static PageResponseInfo Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error001: {0} --- {1}", e.Message,url);
                return null;
            }
            PageResponseInfo page = new PageResponseInfo();            
            StringBuilder buildString = new StringBuilder();
            
            WebRequest request = WebRequest.Create(u);
            request.Timeout = 30000;
            request.Proxy = null;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //WebResponse response = request.GetResponse();
                #region Search and Save
                ////1 set HttpStatus
                page.HttpStatus = response.StatusCode;
                //page.HttpStatus = HttpStatusCode.OK;
                if (page.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    ////Is ErrorPage 
                    if (IsErrorPage(response.ResponseUri.ToString()))
                    {
                        ////2 set IsErrorPage
                        page.IsErrorPage = true;
                        response.Close();
                        return page;
                    }
                    ////Check content
                    else
                    {
                        Stream receiveStream = response.GetResponseStream();
                        Encoding encode = Encoding.GetEncoding("UTF-8");
                        StreamReader readStream = new StreamReader(receiveStream, encode);                        
                        while (!readStream.EndOfStream)
                        {
                            buildString.Append(readStream.ReadLine() + "\n");
                        }
                        ////3 set ResponseBody
                        page.ResponseBody = buildString.ToString();
                        ////2 set IsErrorPage
                        page.IsErrorPage = false;
                        ////4 set ResponseUrl
                        page.ResponseUrl = response.ResponseUri.AbsoluteUri;
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(page.ResponseBody);                        
                        #region Initialize LinkSet
                        LinkInfo link;
                        IList<LinkInfo> linkList = new List<LinkInfo>();
                        HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a");
                        if (nodes == null || nodes.Count == 0)
                        {
                            linkList = new List<LinkInfo>();
                        }
                        else
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                if (node.Attributes["href"] != null)
                                {
                                    string href = node.Attributes["href"].Value.ToLower();
                                    if (!href.Contains("javascript"))
                                    {
                                        link = new LinkInfo();
                                        link.LinkUrl = new Model.Url(UrlHelper.WapperUrl(response.ResponseUri.ToString(), url, href));
                                        link.LinkText = Utils.ClearHTML(node.InnerHtml);
                                        link.ParentUrl = new Model.Url(url);
                                        linkList.Add(link);
                                    }
                                }
                            }
                        }
                        LinkSet linkSet = new LinkSet(linkList);
                        ////5 set LinkSet
                        page.LinkSet = linkSet;
                        #endregion
                        #region Initialize ImgSet
                        Img img;
                        IList<Img> imgList = new List<Img>();
                        HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");
                        if (imgNodes == null || imgNodes.Count == 0)
                        {
                            imgList = new List<Img>();
                        }
                        else
                        {
                            foreach (HtmlNode node in imgNodes)
                            {
                                if (node.Attributes["src"] != null && !string.IsNullOrEmpty(node.Attributes["src"].Value))
                                {
                                    string src = node.Attributes["src"].Value.ToLower();
                                    if (!src.Contains("javascript"))
                                    {
                                        img = new Img();
                                        img.ImgSrc = new Src(UrlHelper.WapperUrl(response.ResponseUri.ToString(), url, src));
                                        img.ParentUrl = new Model.Url(url);
                                        imgList.Add(img);
                                    }
                                }
                            }
                        }

                        ImgSet imgSet = new ImgSet(imgList);
                        ////6 set ImgSet
                        page.ImgSet = imgSet;
                        #endregion
                        readStream.Close();
                        receiveStream.Close();
                        response.Close();
                        return page;
                    }
                }
                else
                {
                    ////2 set IsErrorPage
                    page.IsErrorPage = true;
                    response.Close();
                    return page;
                }
                #endregion
            }
            catch (WebException e)
            {
#if DEBUG
                Console.WriteLine("Error002:  {0} --- {1}", e.Message, url);
#endif
                return null;
            }
        }
                     
        /// <summary>
        /// Select node using XPath 
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="XPath">XPath string</param>
        /// <returns>string list</returns>
        public static List<string> GetNodeInnerText(string url, string XPath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(XPath))
            {
                return null;
            }        
            List<string> InnerText = new List<string>();
            string pageContent = GetPageContent(url);
            if (IsValidWebPage(pageContent))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContent);
                HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(XPath);
                if (nodes == null || nodes.Count == 0)
                {
                    InnerText.Add("ERROR: Not find any node for '" + XPath + "'");
                }
                else
                {
                    foreach (HtmlNode node in nodes)
                    {
                        InnerText.Add(node.InnerText);
                    }
                }
            }
            else
            {
                InnerText.Add(pageContent);
            }
            return InnerText;
        }        

        /// <summary>
        /// Find out a string from a webpage via access a url
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="text">Particular Text</param>
        /// <returns>string list</returns>
        public static List<string> GetParticularText(string url, string text)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error003" + e.Message);
                return null;
            }      
            List<string> PageContents = new List<string>();
            List<string> ReturnLines = new List<string>();
            List<int> Lines = new List<int>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);
            //request.Accept = "*/*";            
            request.Timeout = 30000;
            request.Proxy = null;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                #region Search and Save
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ////Is ErrorPage 
                    if (IsErrorPage(response.ResponseUri.ToString()))
                    {
                        ReturnLines.Add("Get Sorry Page");
                    }
                    ////Check content
                    else
                    {
                        Stream receiveStream = response.GetResponseStream();
                        Encoding encode = Encoding.GetEncoding("UTF-8");
                        StreamReader readStream = new StreamReader(receiveStream, encode);
                        while (!readStream.EndOfStream)
                        {
                            PageContents.Add(readStream.ReadLine());
                        }

                        for (int i = 0; i < PageContents.Count; i++)
                        {                            
                            ////Get the line number for the special text
                            if (PageContents[i].Contains(text))
                            {
                                Lines.Add(i - 1);
                                Lines.Add(i);
                                Lines.Add(i + 1);
                            }
                        }

                        if (Lines == null || Lines.Count < 1)
                        {
                            ReturnLines.Add(string.Format("Don't find '{0}'", text));
                        }
                        else
                        {
                            Lines.Sort();
                            if (Lines[0] < 0)
                            {
                                Lines.RemoveAt(0);
                            }
                            if (Lines[Lines.Count - 1] >= PageContents.Count)
                            {
                                Lines.RemoveAt(Lines.Count - 1);
                            }

                            int j = 1;
                            while (j < Lines.Count)
                            {
                                if (Lines[j] == Lines[j - 1])
                                {
                                    Lines.RemoveAt(j);
                                }
                                else
                                {
                                    j++;
                                }
                            }
                            foreach (int x in Lines)
                            {
                                ReturnLines.Add(PageContents[x]);
                            }
                        }
                    }
                }
                else
                {
                    ReturnLines.Add("ERROR: " + response.StatusCode);
                }
                #endregion
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("Error004: {0}", e.Message);
            }            
            return ReturnLines;
        }

        /// <summary>
        /// return webpage content from a url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>webpage content</returns>
        public static string GetPageContent(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error005" + e.Message);
                return null;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(u);           
            request.Timeout = 30000;
            request.Proxy = null;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                #region Search and Save                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ////Is ErrorPage 
                    if (IsErrorPage(response.ResponseUri.ToString()))
                    {
                        response.Close();
                        return "ERRORPAGE";
                    }
                    ////Check content
                    else
                    {
                        Stream receiveStream = response.GetResponseStream();
                        Encoding encode = Encoding.GetEncoding("UTF-8");
                        StreamReader readStream = new StreamReader(receiveStream, encode);
                        StringBuilder buildString = new StringBuilder();
                        while (!readStream.EndOfStream)
                        {
                            buildString.Append(readStream.ReadLine()+"\n");
                        }
                        readStream.Close();
                        //receiveStream.Close();
                        response.Close();
                        return buildString.ToString();
                    }
                }
                else
                {
                    response.Close();
                    return "ERRORCODE: " + response.StatusCode;
                }
                #endregion                
            }
            catch (WebException e)
            {
                Console.WriteLine("Error006: {0}", e.Message);
                return "WebException";
            }    
        }

        /// <summary>
        /// verify the page is valid
        /// </summary>
        /// <param name="pageContent">page content</param>
        /// <returns>bool</returns>
        private static bool IsValidWebPage(string pageContent)
        {
            return !pageContent.StartsWith("ERRORPAGE: ")
                && !pageContent.StartsWith("ERRORCODE: ")
                && !pageContent.StartsWith("WebException: ");
        }

        /// <summary>
        /// is an error page?
        /// </summary>
        /// <param name="responseUrl">response url</param>
        /// <returns>bool</returns>
        private static bool IsErrorPage(string responseUrl)
        {
            return (responseUrl.ToLower().Contains("404.html")
                        || responseUrl.ToLower().Contains("error.mspx")
                        || responseUrl.ToLower().Contains("error.aspx")
                        || responseUrl.ToLower().Contains("errorpage.aspx"));
        }
    }
}