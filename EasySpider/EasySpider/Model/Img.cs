//-----------------------------------------------------------------------
// <copyright file="Img.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    /// <summary>
    /// Model for Img
    /// </summary>
    public class Img
    {
        /// <summary>
        /// Initializes a new instance of the Img class
        /// </summary>
        public Img()
        {
        }

        /// <summary>
        /// Gets or sets src
        /// </summary>
        public Src ImgSrc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets alt
        /// </summary>
        public string Alt
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets id
        /// </summary>
        public string Id
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets class
        /// </summary>
        public string Class
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
