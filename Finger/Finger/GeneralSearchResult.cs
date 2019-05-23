using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
namespace FingerCheck
{
    public class GeneralSearchResult
    {
        public Header header = new Header();
        private DataTable fieldDefine = new DataTable();
        /// <summary> 
        /// 返回的数据结构定义，无数据 
        /// </summary> 
        public DataTable FieldDefine
        {
            get { return fieldDefine; }
            set { fieldDefine = value; }
        }
        private DataTable retrunData = new DataTable();
        /// <summary> 
        /// 返回的数据，格式为DataTable，结构和FieldDefine中的结构一样 
        /// </summary> 
        public DataTable RetrunData
        {
            get { return retrunData; }
            set { retrunData = value; }
        }
        /// <summary> 
        /// 将json数据转换为定义好的对象，数据转换为DataTable 
        /// </summary> 
        /// <param name="jsonText"></param> 
        /// <returns></returns> 
        public static GeneralSearchResult GetTransformData(string jsonText)
        {
            GeneralSearchResult gsr = new GeneralSearchResult();
            JavaScriptSerializer s = new JavaScriptSerializer();
            Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(jsonText);
            Dictionary<string, object> apps = (Dictionary<string, object>)JsonData["apps"];
            Dictionary<string, object> header = (Dictionary<string, object>)apps["header"];
            Dictionary<string, object> fieldDefine = (Dictionary<string, object>)apps["header"];
            Dictionary<string, object> data = (Dictionary<string, object>)apps["data"];
            object[] rows = (object[])data["row"];
            gsr.header.Version = header["version"].ToString();
            gsr.header.ErrorInfo = header["errorInfo"].ToString();
            gsr.header.ReturnCode = header["returnCode"].ToString();
            gsr.header.ReturnRows = Convert.ToInt16(header["returnRows"]);
            gsr.header.TotalRows = Convert.ToInt16(header["totalRows"]);
            Dictionary<string, object> dicFieldDefine = (Dictionary<string, object>)apps["fieldDefine"];
            foreach (KeyValuePair<string, object> ss in dicFieldDefine)
            {
                gsr.FieldDefine.Columns.Add(ss.Key, typeof(string));
            }
            gsr.RetrunData = gsr.FieldDefine.Clone();
            foreach (object ob in rows)
            {
                Dictionary<string, object> val = (Dictionary<string, object>)ob;
                DataRow dr = gsr.RetrunData.NewRow();
                foreach (KeyValuePair<string, object> sss in val)
                {
                    dr[sss.Key] = sss.Value;
                }
                gsr.RetrunData.Rows.Add(dr);
            }
            return gsr;
        }
        /// <summary> 
        /// 数据文件头定义 
        /// </summary> 
        public class Header
        {
            private string version;
            /// <summary> 
            /// 版本 
            /// </summary> 
            public string Version
            {
                get { return version; }
                set { version = value; }
            }
            private string returnCode;
            /// <summary> 
            /// 结果码，0为正常，否则为有错误 
            /// </summary> 
            public string ReturnCode
            {
                get { return returnCode; }
                set { returnCode = value; }
            }
            private string errorInfo;
            /// <summary> 
            /// 如果ReturnCode为非0时的错误信息 
            /// </summary> 
            public string ErrorInfo
            {
                get { return errorInfo; }
                set { errorInfo = value; }
            }
            private int totalRows;
            /// <summary> 
            /// 查询结果总行数 
            /// </summary> 
            public int TotalRows
            {
                get { return totalRows; }
                set { totalRows = value; }
            }
            private int returnRows;
            /// <summary> 
            /// 返回的数据行数 
            /// </summary> 
            public int ReturnRows
            {
                get { return returnRows; }
                set { returnRows = value; }
            }
        }
    }
}

