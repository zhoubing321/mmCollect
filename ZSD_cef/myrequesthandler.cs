using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ZSD_cef
{
    public partial class requesthandler : IRequestHandler
    {
        //public static string requestUrl = "";//提交前url

        public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            callback.Dispose();
            return false;
        }

        public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
            //throw new NotImplementedException();
        }

        public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            if (request.Url.StartsWith("http"))
            {
                //requestUrl = request.Url;
            }
            return false;
            //throw new NotImplementedException();
        }

        public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            return false;
            //throw new NotImplementedException();
        }

        public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
        {
            //throw new NotImplementedException();
        }

        public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            //throw new NotImplementedException();
        }

        public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            return false;
            //throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 在自己窗口打开链接
    /// </summary>
    internal class CefSharpOpenPageSelf : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            chromiumWebBrowser.Load(targetUrl);
            return true; //Return true to cancel the popup creation copyright by codebye.com.
        }
    }

}
