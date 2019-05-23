//-----------------------------------------------------------------------
// <copyright file="PageResponseInfo.cs" company="CMDI">
//     Copyright (c) 2013 China Mobile Group Design Institute.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasySpider.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// response page model
    /// </summary>
    public class PageResponseInfo
    {
        #region Fileds

        /// <summary>
        /// PageResponseInfo contains a response url info
        /// </summary>
        private string responseUrl;

        /// <summary>
        /// the LinkSet of this page
        /// </summary>
        private LinkSet linkSet;

        /// <summary>
        /// the Http Status Code
        /// </summary>
        private System.Net.HttpStatusCode httpStatus;

        /// <summary>
        /// Is it an Error Page?
        /// </summary>
        private bool isErrorPage;

        /// <summary>
        /// The HTML body (string)
        /// </summary>
        private string responseBody;

        /// <summary>
        /// the ImgSet of this page
        /// </summary>
        private ImgSet imgSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PageResponseInfo class
        /// </summary>
        public PageResponseInfo()
        {
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets response ImgSet
        /// </summary>
        public ImgSet ImgSet
        {
            get { return this.imgSet; }
            set { this.imgSet = value; }
        }

        /// <summary>
        /// Gets or sets response LinkSet
        /// </summary>
        public LinkSet LinkSet
        {
            get { return this.linkSet; }
            set { this.linkSet = value; }
        }

        /// <summary>
        /// Gets or sets response status code
        /// </summary>
        public System.Net.HttpStatusCode HttpStatus
        {
            get { return this.httpStatus; }
            set { this.httpStatus = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is an ErrorPage
        /// </summary>
        public bool IsErrorPage
        {
            get { return this.isErrorPage; }
            set { this.isErrorPage = value; }
        }        

        /// <summary>
        /// Gets or sets response body
        /// </summary>
        public string ResponseBody
        {
            get
            {
                if (!string.IsNullOrEmpty(this.responseBody))
                {
                    return this.responseBody.ToLower();
                }
                else
                {
                    return "";
                }
            }
            set { this.responseBody = value; }
        }

        /// <summary>
        /// Gets access result
        /// </summary>
        public AccessResult Result
        {
            get
            {
                if (System.Net.HttpStatusCode.OK == this.httpStatus && false == this.isErrorPage)
                {
                    return AccessResult.Success;
                }
                else
                {
                    return AccessResult.Fail;
                }
            }
        }

        /// <summary>
        /// Gets or sets Response Url
        /// </summary>
        public string ResponseUrl
        {
            get { return this.responseUrl.ToLower(); }
            set { this.responseUrl = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// verify response contains defined element
        /// </summary>
        /// <param name="element">element tag</param>
        /// <returns>if find it, return true, else return false</returns>
        public bool ContainsElement(string element)
        {
            string startTag = string.Format("<{0}", element.ToLower());
            string endTag = string.Format("</{0}>", element.ToLower());
            return this.ResponseBody.Contains(startTag) && this.ResponseBody.Contains(endTag);
        }
        #endregion
    }

    /// <summary>
    /// enum for test result
    /// </summary>
    public enum AccessResult
    {
        /// <summary>
        /// test result: pass
        /// </summary>
        Success = 0,

        /// <summary>
        /// test result: fail
        /// </summary>
        Fail = 1
    }
}
