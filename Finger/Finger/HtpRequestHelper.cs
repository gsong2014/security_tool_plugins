using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
namespace FingerCheck
{
    /// <summary>
    /// HTTP请求帮助类
    /// </summary>
    public class HtpRequestHelper
    {
        #region  FIELD
        protected HttpWebRequest webRequest = null;
        protected HttpWebResponse webResponse = null;
        public const string UserAgent_IE = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.2; Trident/4.0; .NET CLR 1.1.4322; InfoPath.2; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
        public const string UserAgent_Chrome = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
        public const string UserAgent_FireFox = "Mozilla/5.0 (Windows NT 5.2; rv:18.0) Gecko/20100101 Firefox/18.0";
        #endregion

        #region   PUBLIC FUNCTION
        /// <summary>
        /// 创建HttpWebRequest请求
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="requestItem">请求参数</param>
        /// <param name="responseCallBack"></param>
        /// <returns></returns>
        public T CreateWebRequest<T>(RequestItem requestItem, Func<HttpWebResponse, T> responseCallBack)
        {
            T result = default(T);
            if (null == requestItem)
            {
                throw new ArgumentNullException("请求参数RequestItem为Null");
            }
            if (responseCallBack == null)
            {
                throw new ArgumentNullException("委托responseCallBack为Null");
            }
            try
            {
                SetCertificateParams(requestItem);
                if (null != webRequest)
                {
                    SetWebProxyParams(requestItem);   //设置
                    SetServicePointParams();     //设置请求的办事端信息
                    SetRequestParams(requestItem);    // 设置HTTP请求头,HTTP版本信息等信息
                    SetPostRequestParams(requestItem);    //设置POST请求参数
                    result = GetWebResponse<T>(responseCallBack);    //获取WebResponse响应
                }
                else
                {
                    throw new ArgumentNullException("创建HttpWebRequest请求失败 HttpWebRequest为Null");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseWebResponse();     //封闭响应
                AbortWebRequest();      //作废请求
            }
            return result;
        }
        /// <summary>
        ///  应用POST体式格式请求数据
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        public ResponseItem RequestDataByPost(RequestItem requestItem)
        {
            return RequestDataByPost<ResponseItem>(requestItem, ResponseCallBack);
        }
        /// <summary>
        /// 应用POST体式格式请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestItem"></param>
        /// <param name="responseCallBack"></param>
        /// <returns></returns>
        public T RequestDataByPost<T>(RequestItem requestItem, Func<HttpWebResponse, T> responseCallBack)
        {
            T result = default(T);
            if (null != requestItem)
            {
                requestItem.Method = HttpMethodEnum.POST;
                result = CreateWebRequest(requestItem, responseCallBack);
            }
            else
            {
                throw new ArgumentNullException("POST请求-请求参数RequestItem为Null");
            }
            return result;
        }
        /// <summary>
        /// 应用GET体式格式请求数据
        /// </summary>
        /// <param name="requestItem"></param>
        /// <returns></returns>
        public ResponseItem RequestDataByGet(RequestItem requestItem)
        {
            return RequestDataByGet<ResponseItem>(requestItem, ResponseCallBack);
        }
        /// <summary>
        /// 应用GET体式格式请求数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestItem"></param>
        /// <param name="responseCallBack"></param>
        /// <returns></returns>
        public T RequestDataByGet<T>(RequestItem requestItem, Func<HttpWebResponse, T> responseCallBack)
        {
            T result = default(T);
            if (null != requestItem)
            {
                requestItem.Method = HttpMethodEnum.GET;
                result = CreateWebRequest(requestItem, responseCallBack);
            }
            else
            {
                throw new ArgumentNullException("GET请求-请求参数RequestItem为Null");
            }
            return result;
        }
        #endregion

        #region PROTECTED FUNCTION
        /// <summary>
        /// 设置WebRequest请求证手札息
        /// </summary>
        /// <param name="requestItem"></param>
        protected void SetCertificateParams(RequestItem requestItem)
        {
            if (requestItem.IsNeedCertificate)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }
            webRequest = WebRequest.Create(requestItem.RequestUrl) as HttpWebRequest;
        }
        /// <summary>
        /// 设置HTTP请求
        /// </summary>
        /// <param name="requestItem"></param>
        protected void SetWebProxyParams(RequestItem requestItem)
        {
            if (null != requestItem.ProxyInfo)
            {
                ProxySetInfo proxyInfo = requestItem.ProxyInfo;
                if (!string.IsNullOrEmpty(proxyInfo.ProxyIp))
                {
                    WebProxy webProxy = new WebProxy(proxyInfo.ProxyIp);
                    webProxy.Credentials = new NetworkCredential(proxyInfo.ProxyUserName, proxyInfo.ProxyPassword);
                    webRequest.Proxy = webProxy;
                    webRequest.Credentials = CredentialCache.DefaultCredentials;  //设置安然凭证
                }
            }
        }
        /// <summary>
        ///  设置HTTP请求头,HTTP版本信息等信息
        /// </summary>
        /// <param name="requestItem"></param>
        protected void SetRequestParams(RequestItem requestItem)
        {
            webRequest.Method = requestItem.Method.ToString();   //请求类型
            webRequest.ProtocolVersion = HttpVersion.Version11;  //请求HTTP版本
            webRequest.Timeout = requestItem.Timeout;            //超不时候
            if (requestItem.RequestCookies == null)
            {
                requestItem.RequestCookies = new CookieContainer();
            }
            webRequest.CookieContainer = requestItem.RequestCookies;   //设置与请求接洽关系的Cookies
            webRequest.KeepAlive = requestItem.KeepAlive;     //是否对峙持久链接
            webRequest.AllowAutoRedirect = requestItem.AutoRedirect;       //是否容许主动跳转
            if (!string.IsNullOrEmpty(requestItem.Referer))
            {
                webRequest.Referer = requestItem.Referer;   //请求起原Url
            }
            if (!string.IsNullOrEmpty(requestItem.ContentType))
            {
                webRequest.ContentType = requestItem.ContentType;
            }
            if (!string.IsNullOrEmpty(requestItem.RequestAccept))
            {
                webRequest.Accept = requestItem.RequestAccept;    //设置Accept标头
            }
            if (string.IsNullOrEmpty(requestItem.UserAgent))
            {
                requestItem.UserAgent = UserAgent_IE;
            }
            webRequest.UserAgent = requestItem.UserAgent;  //设置User-Agent标头
            WebHeaderCollection headerColl = requestItem.GetHeaderData();
            if (null != headerColl && headerColl.Count > 0)
            {
                webRequest.Headers.Add(headerColl);
            }
        }
        /// <summary>
        /// 经由过程设备不应用缓冲、最大连接数进步效力
        /// </summary>
        protected void SetServicePointParams()
        {
            webRequest.ServicePoint.ConnectionLimit = 1024;
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.ServicePoint.UseNagleAlgorithm = false;
            webRequest.AllowWriteStreamBuffering = false;
        }
        /// <summary>
        /// 设置POST请求参数
        /// </summary>
        /// <param name="requestItem"></param>
        protected void SetPostRequestParams(RequestItem requestItem)
        {
            if (webRequest.Method.ToLower() == "post")
            {
                if (string.IsNullOrEmpty(webRequest.ContentType))
                {
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                }
                webRequest.ReadWriteTimeout = requestItem.ReadWriteTimeout;
                byte[] buffer = Encoding.UTF8.GetBytes(requestItem.GetParamsData());
                webRequest.ContentLength = buffer.Length;
                Stream writer = webRequest.GetRequestStream();
                writer.Write(buffer, 0, buffer.Length);
                writer.Close();
            }
        }
        #endregion

        #region  PRIVATE FUNCTION
        /// <summary>
        /// 获取WebResponse响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseCallBack"></param>
        /// <returns></returns>
        private T GetWebResponse<T>(Func<HttpWebResponse, T> responseCallBack)
        {
            int webExStatus = 0;    //响应状况
            try
            {
                webResponse = webRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException webEx)
            {
                webExStatus = (int)webEx.Status;
                webResponse = (HttpWebResponse)webEx.Response;
            }
            if (null != webResponse)
            {
                return responseCallBack(webResponse);
            }
            else
            {
                throw new ArgumentNullException("获取HttpWebResponse响应失败 HttpWebResponse为Null,响应状况：" + webExStatus.ToString());
            }
        }
        /// <summary>
        ///  作废HttpWebRequest请求
        /// </summary>
        private void AbortWebRequest()
        {
            if (null != webRequest)
            {
                try
                {
                    webRequest.Abort();
                    webRequest = null;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 封闭HttpWebResponse响应
        /// </summary>
        private void CloseWebResponse()
        {
            if (null != webResponse)
            {
                try
                {
                    webResponse.Close();
                    webResponse = null;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 读取 HttpWebResponse 返回数据
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns></returns>
        private ResponseItem ResponseCallBack(HttpWebResponse webResponse)
        {
            ResponseItem resultItem = new ResponseItem();
            resultItem.CookieColl = webResponse.Cookies;
            resultItem.HeaderColl = webResponse.Headers;
            resultItem.LocationUrl = webResponse.Headers[HttpResponseHeader.Location];
            resultItem.SetCookies = webResponse.Headers[HttpResponseHeader.SetCookie];
            resultItem.StatusCode = webResponse.StatusCode;
            resultItem.StatusMsg = webResponse.StatusDescription;
            Stream stream = webResponse.GetResponseStream();
            int readCount = 0;
            const int length = 1024;
            byte[] bufferBytes = new byte[length];
            MemoryStream memoryStream = new MemoryStream();
            while ((readCount = stream.Read(bufferBytes, 0, length)) > 0)
            {
                memoryStream.Write(bufferBytes, 0, readCount);
            }
            resultItem.ResultBytes = memoryStream.ToArray();
            resultItem.ResultHtml = Encoding.UTF8.GetString(resultItem.ResultBytes, 0, resultItem.ResultBytes.Length);
            memoryStream.Close();
            stream.Close();
            return resultItem;
        }
        #endregion
    }

    /// <summary>
    //RequestItem 请求参数类
    /// </summary>
    public class RequestItem
    {
        #region 字段
        private readonly IDictionary<string, string> dicParamsData = null;
        private readonly WebHeaderCollection headerCollection = null;
        private string reqUrl = string.Empty;
        private CookieContainer reqCookie = new CookieContainer();
        private int timeout = 10000;
        private string reqAccept;
        private string userAgent;
        private string referer = string.Empty;
        private bool autoRedirect = true;
        private bool keepAlive = false;
        private ProxySetInfo proxyInfo = null;
        private string contentType;
        private HttpMethodEnum method = HttpMethodEnum.GET;
        private int readWriteTimeout = 10000;
        #endregion

        #region 机关函数
        public RequestItem()
        {
            dicParamsData = new Dictionary<string, string>();
            headerCollection = new WebHeaderCollection();
        }
        public RequestItem(string url)
            : this()
        {
            this.reqUrl = url;
        }
        public RequestItem(string url, CookieContainer cookie)
            : this(url)
        {
            this.reqCookie = cookie;
        }
        #endregion

        #region  属性
        /// <summary>
        /// 请求链接
        /// </summary>
        public string RequestUrl
        {
            get { return reqUrl; }
            set { reqUrl = value; }
        }
        /// <summary>
        /// Cookie凑集
        /// </summary>
        public CookieContainer RequestCookies
        {
            get { return reqCookie; }
            set { reqCookie = value; }
        }
        /// <summary>
        /// 请求超不时候 单位：毫秒 默认10秒超时
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
        /// <summary>
        /// 请求标头值
        /// </summary>
        public string RequestAccept
        {
            get { return reqAccept; }
            set { reqAccept = value; }
        }
        /// <summary>
        /// 客户端接见信息
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }
        /// <summary>
        /// 是否容许主动跳转
        /// </summary>
        public bool AutoRedirect
        {
            get { return autoRedirect; }
            set { autoRedirect = value; }
        }
        /// <summary>
        /// 起原地址前次接见地址/主动跳转地址
        /// </summary>
        public string Referer
        {
            get { return referer; }
            set { referer = value; }
        }
        /// <summary>
        /// 是否与办事器建树持久链接
        /// </summary>
        public bool KeepAlive
        {
            get { return keepAlive; }
            set { keepAlive = value; }
        }
        /// <summary>
        /// 办事器信息
        /// </summary>
        public ProxySetInfo ProxyInfo
        {
            get { return proxyInfo; }
            set { proxyInfo = value; }
        }
        /// <summary>
        /// 是否设置请求
        /// </summary>
        public bool IsNeedCertificate
        {
            get
            {
                if (!string.IsNullOrEmpty(RequestUrl))
                {
                    return RequestUrl.StartsWith("https:");
                }
                return false;
            }
        }
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        /// <summary>
        ///  HTTP请求类型 默认为GET
        /// </summary>
        public HttpMethodEnum Method
        {
            get { return method; }
            set { method = value; }
        }
        /// <summary>
        /// POST写入数据超不时候  单位：毫秒  默认 10秒
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return readWriteTimeout; }
            set { readWriteTimeout = value; }
        }
        #endregion

        #region 办法
        /// <summary>
        /// 添加HttpWebRequest标头信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeaderData(string name, string value)
        {
            headerCollection.Add(name, value);
        }
        /// <summary>
        /// 添加HttpWebRequest标头信息
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        public void SetHeaderData(HttpRequestHeader header, string value)
        {
            headerCollection.Add(header, value);
        }
        /// <summary>
        /// 获取HttpWebRequest标头信息
        /// </summary>
        /// <returns></returns>
        public WebHeaderCollection GetHeaderData()
        {
            return headerCollection;
        }
        /// <summary>
        /// 添加POST请求
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetParamsData(string name, string value)
        {
            if (!dicParamsData.ContainsKey(name))
            {
                dicParamsData.Add(name, value);
            }
        }
        /// <summary>
        /// 获取POST请求数据
        /// </summary>
        /// <returns></returns>
        public string GetParamsData()
        {
            StringBuilder sbResult = new StringBuilder();
            if (null != dicParamsData)
            {
                bool isTag = true;
                foreach (var current in dicParamsData)
                {
                    if (!isTag)
                    {
                        sbResult.AppendFormat("&{0}={1}", current.Key, current.Value);
                        continue;
                    }
                    else
                    {
                        sbResult.AppendFormat("{0}={1}", current.Key, current.Value);
                        isTag = false;
                    }
                }
            }
            return sbResult.ToString();
        }
        #endregion
    }
    public class ProxySetInfo
    {
        private string proxyUserName = string.Empty;
        /// <summary>
        /// 办事器 用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyUserName; }
            set { proxyUserName = value; }
        }
        private string proxyPassword = string.Empty;
        /// <summary>
        /// 办事器 暗码
        /// </summary>
        public string ProxyPassword
        {
            get { return proxyPassword; }
            set { proxyPassword = value; }
        }
        private string proxyIp = string.Empty;
        /// <summary>
        /// 办事器 IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyIp; }
            set { proxyIp = value; }
        }
    }
    /// <summary>
    /// HTTP请求类型
    /// </summary>
    public enum HttpMethodEnum
    {
        /// <summary>
        /// GET请求
        /// </summary>
        GET = 0,
        /// <summary>
        /// POST请求
        /// </summary>
        POST = 1
    }
    //ResponseItem 返回数据类
    public class ResponseItem
    {
        private CookieCollection cookieColl = new CookieCollection();
        /// <summary>
        ///WebResponse.Cookies 凑集
        /// </summary>
        public CookieCollection CookieColl
        {
            get { return cookieColl; }
            set { cookieColl = value; }
        }
        private string setCookies = string.Empty;
        /// <summary>
        /// WebResponse.Headers Set-Cookie 
        /// </summary>
        public string SetCookies
        {
            get { return setCookies; }
            set { setCookies = value; }
        }
        private WebHeaderCollection headerColl = new WebHeaderCollection();
        /// <summary>
        /// Header
        /// </summary>
        public WebHeaderCollection HeaderColl
        {
            get { return headerColl; }
            set { headerColl = value; }
        }
        private string resultHtml;
        /// <summary>
        /// 返回HTML字符串
        /// </summary>
        public string ResultHtml
        {
            get { return resultHtml; }
            set { resultHtml = value; }
        }
        private byte[] resultBytes = null;
        /// <summary>
        /// 返回字节俭
        /// </summary>
        public byte[] ResultBytes
        {
            get { return resultBytes; }
            set { resultBytes = value; }
        }
        private string locationUrl = string.Empty;
        /// <summary>
        ///WebResponse.Headers 主动跳转链接
        /// </summary>
        public string LocationUrl
        {
            get { return locationUrl; }
            set { locationUrl = value; }
        }
        private HttpStatusCode statusCode;
        /// <summary>
        /// Http响应状况码
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
        private string statusMsg;
        /// <summary>
        /// Http响应信息
        /// </summary>
        public string StatusMsg
        {
            get { return statusMsg; }
            set { statusMsg = value; }
        }
    }
}
