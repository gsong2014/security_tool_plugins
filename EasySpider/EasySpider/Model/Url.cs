//-----------------------------------------------------------------------
// <copyright file="Url.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Manipulate the Url
    /// </summary>
    public class Url
    {
        /// <summary>
        /// the url value
        /// </summary>
        private string data;

        /// <summary>
        /// Initializes a new instance of the Url class
        /// </summary>
        /// <param name="url">a string of url</param>
        public Url(string url)
        {
            this.data = url;
        }

        /// <summary>
        /// Gets or sets url value
        /// </summary>
        public string Data
        {
            get
            {
                return ConvertUrl(this.data).ToLower();
            }

            set
            {
                this.data = value;
            }
        }

        /// <summary>
        /// convert the url to the right one accroding to test environment
        /// </summary>
        /// <param name="url">the original url</param>
        /// <returns>the converted url</returns>
        private static string ConvertUrl(string url)
        {
            if (url == string.Empty)
            {
                return url;
            }
            return url.Trim();
        }
    }
}
