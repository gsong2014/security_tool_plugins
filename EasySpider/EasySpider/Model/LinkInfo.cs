//-----------------------------------------------------------------------
// <copyright file="LinkInfo.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    /// <summary>
    /// Model for link
    /// </summary>
    public class LinkInfo
    {
        /// <summary>
        /// Initializes a new instance of the LinkInfo class
        /// </summary>
        public LinkInfo()
        {
        }

        /// <summary>
        /// Gets or sets Link Url
        /// </summary>
        public Url LinkUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets text of a link
        /// </summary>
        public string LinkText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Parent Url
        /// </summary>
        public Url ParentUrl
        {
            get;
            set;
        }
    }
}
