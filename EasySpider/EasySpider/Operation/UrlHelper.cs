//-----------------------------------------------------------------------
// <copyright file="UrlHelper.cs" company="CMDI">
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

    /// <summary>
    /// Url Helper Class
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// Wapper Url
        /// </summary>
        /// <param name="rootURL">response url</param>
        /// <param name="requestUrl">request url</param>
        /// <param name="specialURL">relative url. eg. /Showcase.aspx</param>
        /// <returns>Wappered Url</returns>
        public static string WapperUrl(string rootURL, string requestUrl, string specialURL)
        {
            if (specialURL == "/" || specialURL == "#" || specialURL == "")
            {
                return requestUrl;
            }

            if (specialURL.ToLower().StartsWith("http:"))
            {
                return specialURL;
            }

            if (specialURL.StartsWith("../../../"))
            {
                string tempUrl = rootURL.Substring(0, rootURL.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.EndsWith("/") ? tempUrl.Substring(0, tempUrl.Length - 1) : tempUrl;
                tempUrl = tempUrl + specialURL.Replace("../../../", "/");
                return tempUrl;
            }

            if (specialURL.StartsWith("../../"))
            {
                string tempUrl = rootURL.Substring(0, rootURL.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.EndsWith("/") ? tempUrl.Substring(0, tempUrl.Length - 1) : tempUrl;
                tempUrl = tempUrl + specialURL.Replace("../../", "/");
                return tempUrl;
            }

            if (specialURL.StartsWith("../"))
            {
                string tempUrl = rootURL.Substring(0, rootURL.LastIndexOf('/'));
                tempUrl = tempUrl.Substring(0, tempUrl.LastIndexOf('/'));
                tempUrl = tempUrl.EndsWith("/") ? tempUrl.Substring(0, tempUrl.Length - 1) : tempUrl;
                tempUrl = tempUrl + specialURL.Replace("../", "/");
                return tempUrl;
            }

            if (specialURL.StartsWith("/"))
            {
                rootURL = rootURL.Replace("http://", string.Empty);
                rootURL = rootURL.Substring(0, rootURL.IndexOf('/'));
                string tempUrl = string.Format("http://{0}/{1}", rootURL, specialURL.Substring(1, specialURL.Length - 1));
                return tempUrl;
            }
            
            string url = rootURL.Substring(0, rootURL.LastIndexOf('/')) + '/' + specialURL;            
            return url;
        }
    }
}
