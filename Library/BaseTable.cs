using System;
using System.Collections.Generic;
using System.IO;

namespace Library
{
    public class BaseTable
    {
        public static List<string> SjWebsite = new List<string>();//sj网址
        public static List<string> keyWords = new List<string>();//重点词
        public static List<string> commonWords = new List<string>();//普通词
        public static List<string> BrandWordList = new List<string>();//品牌词
        public static List<string> BiddingWordList = new List<string>();//竞价词

        public static List<string> SjMismatchList = new List<string>();//sj网址不匹配集合

        /// <summary>
        /// 基表创建和读取
        /// </summary>
        public static void fileCreate()
        {
            try
            {
                string AppPath = Directory.GetCurrentDirectory();
                string BaseTablePath = AppPath.Substring(0, AppPath.LastIndexOf('\\'));
              
                SjWebsite = new List<string>(File.ReadAllLines(BaseTablePath + "//网址网站sjzk.txt", System.Text.Encoding.Default));//读取本地所有sj网站
                #region SJ网址不匹配集合
                foreach (string SjDomain in SjWebsite)
                {
                    if (SjDomain.Length > 0)
                    {
                        string domain = "";
                        if (SjDomain.Contains("/"))
                        {
                            domain = SjDomain.Split('/')[0];
                        }
                        else
                        {
                            domain = SjDomain;
                        }
                        if (!SjMismatchList.Contains(domain))
                        {
                            SjMismatchList.Add(domain);
                        }
                    }
                }
                #endregion
                keyWords = new List<string>(File.ReadAllLines(BaseTablePath + "//重点词zk.txt", System.Text.Encoding.Default));//读取本地重点词
                commonWords = new List<string>(File.ReadAllLines(BaseTablePath + "//普通词zk.txt", System.Text.Encoding.Default));//读取本地普通词
                BrandWordList = new List<string>(File.ReadAllLines(BaseTablePath + "//品牌词zk.txt", System.Text.Encoding.Default));//读取本地品牌词
                BiddingWordList = new List<string>(File.ReadAllLines(BaseTablePath + "//竞价词zk.txt", System.Text.Encoding.Default));//读取本地竞价词
            }
            catch (Exception ex)
            {
                Log.LogSave(Directory.GetCurrentDirectory()+ "\\运行日志\\异常日志.txt", "获取基表文件出错【" + ex.Message + "】");
            }
        }





    }
}
