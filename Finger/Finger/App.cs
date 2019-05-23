using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerCheck
{
    class App
    {
        private string appname;

        public string Appname
        {
            get { return appname; }
            set { appname = value; }
        }

        public List<string> Cats = new List<string>();

        public Dictionary<string, object> attr = new Dictionary<string, object>();

        public Dictionary<string, string> reg = new Dictionary<string, string>();

        //官方网站
        private string website;

        public string Website
        {
            get { return website; }
            set { website = value; }
        }

        //网址
        private string html;

        public string Html
        {
            get { return html; }
            set { html = value; }
        }

        //meta
        private string meta;

        public string Meta
        {
            get { return meta; }
            set { meta = value; }
        }

        //脚本
        private string script;

        public string Script
        {
            get { return script; }
            set { script = value; }
        }

        //touxinxi
        private string headers;

        public string Headers
        {
            get { return headers; }
            set { headers = value; }
        }

        private string env;

        public string Env
        {
            get { return env; }
            set { env = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string implies;

        public string Implies
        {
            get { return implies; }
            set { implies = value; }
        }
    }
}
