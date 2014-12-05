//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Operation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// Utiles contains help function
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Cleanup HTML content
        /// </summary>
        /// <param name="htmlstring">a string of html page</param>
        /// <returns>HTML page content</returns>
        public static string ClearHTML(string htmlstring)
        {
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", string.Empty, RegexOptions.IgnoreCase);

            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            htmlstring = regex.Replace(htmlstring, string.Empty);
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", string.Empty, RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", string.Empty, RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);

            htmlstring.Replace("<", string.Empty);
            htmlstring.Replace(">", string.Empty);
            htmlstring.Replace("\r\n", string.Empty);

            return htmlstring;
        }
    }
}
