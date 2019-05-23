//-----------------------------------------------------------------------
// <copyright file="Src.cs" company="CMDI">
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
    /// Manipulate the src of img
    /// </summary>
    public class Src
    {
        /// <summary>
        /// the src of img
        /// </summary>
        private string data;

        /// <summary>
        /// Initializes a new instance of the Src class
        /// </summary>
        /// <param name="src">a string of src</param>
        public Src(string src)
        {
            this.data = src;
        }

        /// <summary>
        /// Gets or sets src value
        /// </summary>
        public string Data
        {
            get
            {
                return ConvertSrc(this.data).ToLower();
            }

            set
            {
                this.data = value;
            }
        }

        /// <summary>
        /// convert the src to the right one accroding to test environment
        /// </summary>
        /// <param name="src">the original src</param>
        /// <returns>the converted src</returns>
        private static string ConvertSrc(string src)
        {            
            if (src == string.Empty)
            {
                return src;
            }            
            return src.Trim();
        }
    }
}
