using CefSharp;
using CefSharp.WinForms;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace ZSD_cef
{
    public static class CefExample
    {
        public static string UserAgentUrl = "http://119.254.92.102/s/zsd2020/UserAgent.txt";//浏览器标识
        private static readonly bool DebuggingSubProcess = Debugger.IsAttached;

        public static void Init()
        {
            var settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            settings.Locale = "zh-CN";
            try
            {
                WebClient client = new WebClient();
                string html = client.DownloadString(UserAgentUrl);
                string[] UaList = html.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (UaList.Length > 0)
                {
                    Random random = new Random();
                    int n = random.Next(0, UaList.Length - 1);   //生成0-集合长度之间的随机数
                    settings.UserAgent = UaList[n];
                }
                else
                {
                    settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";
                }
            }
            catch
            {
                settings.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1_2 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D257 Safari/9537.53";
            }

            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync
            //settings.CefCommandLineArgs.Add("disable-direct-write", "1");
            //settings.MultiThreadedMessageLoop = false;

            settings.CefCommandLineArgs.Add("ppapi-flash-path", System.AppDomain.CurrentDomain.BaseDirectory + "pepflashplayer.dll"); //指定flash的版本，不使用系统安装的flash版本
            settings.CefCommandLineArgs.Add("ppapi-flash-version", "29.0.0.171");
            settings.CefCommandLineArgs.Add("enable-media-stream", "enable-media-stream");
            settings.IgnoreCertificateErrors = true;
            settings.LogSeverity = LogSeverity.Verbose;

            //CefSharp.Cef.Initialize(settings);

            if (DebuggingSubProcess)
            {
                //var architecture = Environment.Is64BitProcess ? "x64" : "x86";
                //settings.BrowserSubprocessPath = "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
            }

            //settings.RegisterScheme(new CefCustomScheme
            //{
            //    SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
            //    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            //});

            if (!Cef.Initialize(settings))
            {
                if (Environment.GetCommandLineArgs().Contains("--type=renderer"))
                {
                    Environment.Exit(0);
                }
                else
                {
                    return;
                }
            }
        }
    }
}