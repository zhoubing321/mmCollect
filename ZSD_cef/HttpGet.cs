using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSD_cef
{
    public class HttpGet
    {
        /// <summary>
        /// GET提交
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGET(string url)
        {
            try
            {
                Library.HttpHelper http = new Library.HttpHelper();
                Library.HttpItem item = new Library.HttpItem();
                item.URL = url;
                item.ResultType = ResultType.String;
                item.Method = "GET";
                item.Timeout = 20000;//超时时间
                HttpResult result = new HttpResult();
                result = http.GetHtml(item);//获取页面方法
                return result.Html;
            }
            catch
            {
                return "";
            }
        }
    }
}
