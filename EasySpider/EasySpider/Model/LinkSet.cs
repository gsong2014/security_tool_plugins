//-----------------------------------------------------------------------
// <copyright file="LinkInfo.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Link Collection Model
    /// </summary>
    public class LinkSet
    {
        /// <summary>
        /// this is a list of LinkInfo
        /// </summary>
        private IList<LinkInfo> linkList;

        /// <summary>
        /// Initializes a new instance of the LinkSet class
        /// </summary>
        /// <param name="linkList">a list of LinkInfo</param>
        public LinkSet(IList<LinkInfo> linkList)
        {
            this.linkList = linkList;
        }

        /// <summary>
        /// Gets or sets the LinkInfo list
        /// </summary>
        public IList<LinkInfo> LinkList
        {
            get { return this.linkList; }
            set { this.linkList = value; }
        }

        /// <summary>
        /// Gets the total count of a list
        /// </summary>
        public int TotalCount
        {
            get { return this.linkList.Count; }
        }

        /// <summary>
        /// Get a LinkSet instance by LinkName
        /// </summary>
        /// <param name="text">the link text</param>
        /// <returns>a new instance of LinkSet</returns>
        public LinkSet GetByLinkText(string text)
        {
            IList<LinkInfo> links = new List<LinkInfo>();
            foreach (LinkInfo linkInfo in this.linkList)
            {
                if (linkInfo.LinkText == text)
                {
                    links.Add(linkInfo);
                }
            }

            return new LinkSet(links);
        }
    }
}
