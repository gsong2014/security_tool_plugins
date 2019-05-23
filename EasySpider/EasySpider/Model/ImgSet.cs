//-----------------------------------------------------------------------
// <copyright file="ImgSet.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Img Collection Model
    /// </summary>
    public class ImgSet
    {
        /// <summary>
        /// this is a list of Img
        /// </summary>
        private IList<Img> imgList;

        /// <summary>
        /// Initializes a new instance of the ImgSet class
        /// </summary>
        /// <param name="imgList">a list of Img</param>
        public ImgSet(IList<Img> imgList)
        {
            this.imgList = imgList;
        }

        /// <summary>
        /// Gets or sets the Img list
        /// </summary>
        public IList<Img> ImgList
        {
            get { return this.imgList; }
            set { this.imgList = value; }
        }

        /// <summary>
        /// Gets the total count of a list
        /// </summary>
        public int TotalCount
        {
            get { return this.imgList.Count; }
        }
    }
}
