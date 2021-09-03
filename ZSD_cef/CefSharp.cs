using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;
using Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ZSD_cef
{
    public partial class CefSharp : Form
    {
        public CefSharp()
        {
            InitializeComponent();
            CefExample.Init();//初始化内核
        }
        private ChromiumWebBrowser CWebBrowser;
        public static string pathNow = Directory.GetCurrentDirectory() + "\\运行日志\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";//运行日志地址;
        public static System.Timers.Timer Timer = new System.Timers.Timer(1000);//超时30秒
        public static System.Timers.Timer TimerOver = new System.Timers.Timer(1000);//下页加载时间
        public static System.Timers.Timer TimerTarget = new System.Timers.Timer(1000);//目标页加载时间
        public static System.Timers.Timer TimeOut = new System.Timers.Timer(1000);//启动卡死
        public static Model.tb_task model_task = new Model.tb_task();//任务集合类
        public static Model.tb_task_log model_task_Log = new Model.tb_task_log();//任务日志集合类
        public static string TaskPath = Directory.GetCurrentDirectory() + "\\Task.txt";//任务文本
        public static string localIP = "";
        public static string IP = "";
        public static string formText = "";
        public static string user_email = "";


        public static string phoneUser = "";//手机号
        public static string phonePwd = "";//密码


        public static List<string> wordsList = new List<string>();//要上传的域名匹配的关键词集合

        public int outerHtmlType = 0;//网页源码状态
        public string outerHtml = "";//网页源码

        public int ButtonType = 0;//点击按钮状态
        public int ButtonCount = 0;//刷新按钮次数
        //public bool isRun = false;//多线程跳出的判断
        public bool isTarget = false;//是否点击到目标页
        public bool targetIsOver = false;//判断目标页是否完成
        public int TargetType = 0;//点击目标状态
        public int TargetCount = 0;//刷新目标页次数

        public int repeadCount = 0;//没有下一页重复次数
        public bool pageIsOver = false;//判断下一页是否完成
        public string LastPage = "";//上一页的网页
        public int loadCount = 0;//刷新次数
        public int HaveMoveClick = 0;//是否有更多搜索的点击
        public int reloadCount = 0;//更多搜索结果刷新次数
        public int moreSearchType = 0;//更多搜索的状态
        public int moreSearchCount = 0;//点击更多搜索的个数

        public int HaveBaiduSafe = 0;//页面是否有百度安全验证
        public int BaiduSafeCount = 0;//百度安全验证出现次数
        public int BaiduSafeSuc = 0;//百度验证是否成功

        public int NextReloadCount = 0;//下一页刷新次数
        public int HaveNextPage = 0;//是否有下一页链接

        public string baduSafeHtml = "";//百度验证后的源码
        public int timeOutCount = 0;//超时次数

        public string address = "";//网页地址
        Dictionary<string, int> PageOverDic = new Dictionary<string, int>();//已点击的网页和次数

        public string wechatApi = "";//微信接口

        List<string> validList = new List<string>();


        bool closePage = false;


        #region 窗体置顶

        //调用API
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow(); //获得本窗体的句柄
        //[System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
        public IntPtr Handle1;
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Handle1 = this.Handle;

                #region 程序启动开始判断是否卡死
                TimeOut = new System.Timers.Timer(180000);
                TimeOut.Elapsed += new System.Timers.ElapsedEventHandler(AppClose);
                #endregion

                #region 判断网页超时
                Timer = new System.Timers.Timer(120000);
                Timer.Elapsed += new System.Timers.ElapsedEventHandler(refresh);
                #endregion

                #region 判断加载完成
                //TimerOver = new System.Timers.Timer(15000);
                //TimerOver.Elapsed += new System.Timers.ElapsedEventHandler(PageOver);
                #endregion

                #region 目标页加载完成
                //TimerTarget = new System.Timers.Timer(15000);
                //TimerTarget.Elapsed += new System.Timers.ElapsedEventHandler(TargetOver);
                #endregion

                #region 读取微信提醒接口地址
                string wechatPath = Directory.GetCurrentDirectory() + "\\微信提醒接口地址.txt";
                if (!File.Exists(wechatPath))
                {
                    File.WriteAllText(wechatPath, "", Encoding.Default);
                }
                wechatApi = File.ReadAllText(wechatPath, Encoding.Default);
                if (wechatApi == "")
                {
                    wechatApi = "http://114.242.22.122:1243/action/sendmsg.ashx?pwd=gfUg9mXoP25XA036d&SendClass=真实点错误点击提醒&msgs=";
                }
                #endregion

                //读取本地的基表数据  
                Library.BaseTable.fileCreate();

                #region  读取本地任务
                if (File.Exists(TaskPath))
                {
                    //string[] taskList = File.ReadAllText(TaskPath, Encoding.UTF8).Split('|');
                    string[] taskList = File.ReadAllLines(TaskPath);
                    if (taskList.Length > 2)
                    {
                        //model_task.task_id = Convert.ToInt32(taskList[0]);//关键词ID
                        //model_task.task_word = taskList[1].Trim();//关键词
                        //model_task.task_domain = taskList[2].Trim();//域名
                        //localIP = taskList[4];//内网IP
                        //IP = taskList[5];//外网IP
                        //formText = taskList[6];//版本号
                        //user_email = taskList[7];//当前账号
                        phoneUser = taskList[0];//手机号
                        phonePwd = taskList[1];//密码
                        model_task.task_word = taskList[2].Trim();//关键词

                        Thread thread = new Thread(delegate ()
                        {
                            OpenPage("https://acc.maimai.cn/login");//打开浏览器和指定网站  
                        });
                        thread.Start();
                    }
                    else
                    {
                        Label_Status("本地任务集合不匹配，清空本地任务重新领取任务！", Color.Red);
                    }
                }
                else
                {
                    Label_Status("本地任务集合不匹配，清空本地任务重新领取任务！", Color.Red);
                }
                #endregion
            }
            catch (Exception err)
            {
                Label_Status("程序出现错误：" + err.Message, Color.Red);
            }
        }

        /// <summary>
        /// 打开网址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="e"></param>
        public void OpenPage(string url)
        {
            Label_Status("打开搜索首页...", Color.Black);
            TimeOut.Start();
            CWebBrowser = new ChromiumWebBrowser(url);
            CWebBrowser.Dock = DockStyle.Fill;
            CWebBrowser.FrameLoadStart += Browser_FrameLoadStart;
            CWebBrowser.FrameLoadEnd += Browser_Page;
            CWebBrowser.LifeSpanHandler = new CefSharpOpenPageSelf();//不弹出窗口
                                                                     //CWebBrowser.RequestHandler = new requesthandler();//重写过滤图片
                                                                     //CWebBrowser.LifeSpanHandler = new OpenPageSelf();//重写打开窗口
            this.panel1.Invoke(new Action(delegate { this.panel1.Controls.Add(CWebBrowser); }));
        }

        /// <summary>
        /// 页面加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Browser_Page(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain)
            {
                return;
            }
            try
            {
                GC.Collect();
                await CWebBrowser.GetSourceAsync();
                //SafeSuc:
                #region  网页的源码
                outerHtml = "";
                OuterHtml();//获取网页源码
                int oh = 0;
                while (oh < 360)
                {
                    oh++;
                    if (outerHtml == "")//没有开始
                    {
                        Thread.Sleep(500);
                    }
                    else if (outerHtml.Contains("没有获取到源码"))//没有百度源码
                    {
                        address = CWebBrowser.Address;//当前网页的网址
                        var result = await CWebBrowser.GetSourceAsync();
                        outerHtml = result.ToString();
                        pageRun();
                        break;
                    }
                    else if (outerHtml.Length > 10)//获取到源码
                    {
                        pageRun();
                        break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Label_Status("程序运行出现异常：" + ex.Message, Color.Red);
                TaskEnd();//程序结束
            }
        }

        /// <summary>
        /// 页面加载完成-处理方法
        /// </summary>
        public void pageRun()
        {
            //lock ("run")
            //{
                Timer.Stop();//2分钟网页超时结束
                TimerOver.Stop();//10秒网页提前打开结束
                TimeOut.Stop();//首页打开卡死结束

                string result = outerHtml;//网页源码

                #region 网址显示
                this.Invoke((EventHandler)(delegate
                {
                    try
                    {
                        textBox_Url.Text = CWebBrowser.Address;//网址
                        Regex titleRegex = new Regex("(?<=<title>).*?(?=</title>)");
                        tabControl1.TabPages[0].Text = titleRegex.Match(result).Value;//标题
                    }
                    catch
                    {
                        tabControl1.TabPages[0].Text = "";
                    }
                }));
                #endregion

                #region 登录页加载
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode indexnode = doc.DocumentNode.SelectSingleNode("//div[@class='loginPhone']");//首页Logo标签
                if (indexnode != null)
                {
                    Label_Status("登录页加载完成！", Color.Green);
                    Thread.Sleep(2000);
                    SetForegroundWindow(Handle1);//置顶
                    Label_Status("设置账号:" + model_task.task_word, Color.Black);
                    //设置搜索框内容
                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementsByName('m')[0].value='" + phoneUser + "'");

                    Label_Status("设置密码:" + model_task.task_word, Color.Black);
                    //设置搜索框内容
                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementsByName('p')[0].value='" + phonePwd + "'");

                    Label_Status("点击登录按钮", Color.Black);
                    #region  获取点击坐标

                    ButtonClick();
                    int bc = 0;
                    while (bc < 240)
                    {
                        bc++;
                        if (ButtonType == 1)//验证通过
                        {
                            ButtonType = 0;
                            break;
                        }
                        else if (ButtonType == -1)//验证失败
                        {
                            ButtonType = 0;
                            Label_Status("没有获取到按钮的链接，刷新重试！", Color.Red);
                            ButtonCount++;
                            if (ButtonCount >= 3)
                            {
                                Label_Status("连续3次没有获取到按钮的链接，程序关闭！", Color.Red);
                                TaskEnd();//程序结束
                            }
                            PageOverDic.Remove(address);//后退和刷新删掉本页链接
                            CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                            Timer.Start();
                            TimerOver.Start();
                            pageIsOver = false;
                            return;
                        }
                        else if (ButtonType == 0)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    #endregion
                    Label_Status("登录加载中...", Color.Black);
                }
                #endregion
                else
                {
                    HtmlNode htmlresult = doc.DocumentNode.SelectSingleNode("//div[@class='v6-titleBar__main']");
                    if (htmlresult != null)
                    {
                        #region 搜索关键词第一页

                        Label_Status("开始获取网页源码...", Color.Black);

                        //string html = PageResult(htmlresult.OuterHtml);

                        rankingManage(doc.ParsedText);
                        //if (html != "")
                        //{
                        //    string resultPath = Directory.GetCurrentDirectory() + "\\结果\\";
                        //    if (Directory.Exists(resultPath))
                        //    {
                        //        Label_Status("网页源码写入本地保存...", Color.Black);
                        //        File.WriteAllText(resultPath + "\\" + model_task.task_word + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt", html);

                        //        Label_Status("程序运行完成，关闭程序！", Color.Black);
                        //        TaskEnd();//程序结束
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        #region 关键词搜索
                        HtmlNode htmlword = doc.DocumentNode.SelectSingleNode("//input[@class='webimSearchInput']");//详情页搜索结果
                        if (htmlword != null)
                        {
                            if (pageIsOver)
                            {
                                return;//由页面刷新结束执行之后操作
                            }
                            pageIsOver = true;


                            Label_Status("点击搜索关键词框", Color.Black);
                            #region  获取点击坐标

                            InputClick();
                            int bc = 0;
                            while (bc < 240)
                            {
                                bc++;
                                if (ButtonType == 1)//验证通过
                                {
                                    ButtonType = 0;
                                    break;
                                }
                                else if (ButtonType == -1)//验证失败
                                {
                                    ButtonType = 0;
                                    Label_Status("没有获取到按钮的链接，刷新重试！", Color.Red);
                                    ButtonCount++;
                                    if (ButtonCount >= 3)
                                    {
                                        Label_Status("连续3次没有获取到按钮的链接，程序关闭！", Color.Red);
                                        TaskEnd();//程序结束
                                    }
                                    PageOverDic.Remove(address);//后退和刷新删掉本页链接
                                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                                    Timer.Start();
                                    TimerOver.Start();
                                    pageIsOver = false;
                                    return;
                                }
                                else if (ButtonType == 0)
                                {
                                    Thread.Sleep(500);
                                }
                            }
                            #endregion

                            for (int i = 0; i < 3; i++)
                            {
                                Label_Status("开始设置搜索关键词" + model_task.task_word, Color.Black);
                                try
                                {
                                    //设置关键词
                                    SendKeys.SendWait("" + model_task.task_word + "");

                                    Label_Status("关键词设置完成", Color.Black);
                                    Thread.Sleep(3000);
                                    //回车事件
                                    SendKeys.SendWait("{Enter}");
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Label_Status("关键词设置出现异常,重新再次设置：" + ex.Message, Color.Red);
                                }
                            }
                        }
                        #endregion
                        else
                        {
                            #region 打开详情页
                            HtmlNode htmlDetailsPage = doc.DocumentNode.SelectSingleNode("//div[@class='PCfeedDetailTxt']");//详情页搜索结果
                            if (htmlDetailsPage != null)
                            {
                                string detailsHtml = htmlDetailsPage.InnerText;

                                #region 关键词判断
                                if (model_task.task_word.Contains("*"))
                                {
                                    string word = model_task.task_word.Split('*')[0];
                                    string pipeiWord = model_task.task_word.Split('*')[1];

                                    if (model_task.task_word.Contains("$"))
                                    {
                                        string[] pipeiList = pipeiWord.Split('$');
                                        for (int p = 0; p < pipeiList.Length; p++)
                                        {
                                            if (detailsHtml.Contains(pipeiList[p]))
                                            {
                                                validList.Add(detailsHtml);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (detailsHtml.Contains(pipeiWord))
                                        {
                                            validList.Add(detailsHtml);
                                        }
                                    }
                                }
                                else if (model_task.task_word.Contains("|"))
                                {
                                    if (detailsHtml.Contains(model_task.task_word))
                                    {
                                        validList.Add(detailsHtml);
                                    }
                                }
                                else
                                {
                                    validList.Add(detailsHtml);
                                }
                                #endregion

                                CWebBrowser.GetBrowser().GoBack();//后退
                                closePage = true;
                            }
                            #endregion
                        }
                    }
                }
            //}
        }


        string html = "";
        /// <summary>
        /// JS网页的源码
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="model_task_task_domain"></param>
        public string PageResult(string html)
        {
            string rehtml = "";
        refresh: CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight);");//滚动到底部
            Thread.Sleep(5000);

            #region  JS网页的源码
            outerHtml = "";
            OuterHtml();//获取网页源码
            int oh = 0;
            while (oh < 360)
            {
                oh++;
                if (outerHtml == "")//没有开始
                {
                    Thread.Sleep(500);
                }
                else if (outerHtml.Contains("没有获取到源码"))//没有百度源码
                {
                    Browser_Page(null, null);
                    break;
                }
                else if (outerHtml.Length > 10)//获取到源码
                {
                    if (outerHtml == html)
                    {
                        Label_Status("网页源码已是最后一页!", Color.Black);
                        rehtml = outerHtml;
                    }
                    else
                    {

                        Label_Status("开始翻下一页...", Color.Black);
                        html = outerHtml;
                        Thread.Sleep(3000);
                        goto refresh;
                    }
                    break;
                }
            }
            #endregion
            return rehtml;
        }
        /// <summary>
        /// 详情页点击
        /// </summary>
        /// <param name="outerHtml"></param>
        public void rankingManage(string outerHtml)
        {

            Thread t = new Thread(ThreadStart(delegate ()

              {

              }));
            t.Start();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(outerHtml);
            //HtmlNode htmlresult = doc.DocumentNode.SelectSingleNode("//div[@class='searchList']");//详情页搜索结果
            HtmlNode htmlresult = doc.DocumentNode.SelectSingleNode("//ul[@class='list-group']");//详情页搜索结果
            if (htmlresult != null)
            {
                HtmlAgilityPack.HtmlNodeCollection nodecollection = htmlresult.ChildNodes;
                //nodecollection.RemoveAt(0);//删除第一条数据
                for (int i = 0; i < nodecollection.Count; i++)
                {
                    string result = nodecollection[i].OuterHtml.ToLower();
                    if (result.Length > 300)
                    {
                        #region 点击详情页
                        if (result.Contains("pingback-item-"))
                        {
                            Regex r = new Regex("(?=pingback-item-).*?(?=\">)");
                            Match m = r.Match(result);
                            if (m.Success)
                            {
                                DetailsPageClick(m.Value);//点击详情页

                                #region  点击详情页
                                int lc = 0;
                                while (true)
                                {
                                    lc++;
                                    if (lc >= 360)
                                    {
                                        TaskEnd();//程序结束
                                        break;
                                    }
                                    if (TargetType == 1)//验证通过
                                    {
                                        TargetType = 0;
                                        break;
                                    }
                                    else if (TargetType == -1)//验证失败
                                    {
                                        TargetType = 0;
                                        TargetCount++;
                                        if (TargetCount >= 3)
                                        {
                                            Label_Status("连续3次没有获取到目标页的链接，程序关闭！", Color.Red);
                                            HttpGet.HttpGET(wechatApi + "SJ端：账号：" + user_email + "点击ID：" + model_task.task_id + "，关键词：" + model_task.task_word + "，域名：" + model_task.task_domain + "连续3次没有获取到目标页的链接！");
                                            TaskEnd();//程序结束
                                        }
                                        PageOverDic.Remove(address);//后退和刷新删掉本页链接
                                        Label_Status("没有获取到目标页的链接，刷新重试！", Color.Red);
                                        CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                                        Timer.Start();
                                        TimerOver.Start();
                                        pageIsOver = false;
                                        return;
                                    }
                                    else if (TargetType == 0)
                                    {
                                        Thread.Sleep(500);
                                    }
                                }
                                #endregion

                                while (!closePage)
                                {
                                    Thread.Sleep(3000);
                                }
                            }
                        }
                        #endregion
                    }
                }
                //滚动到底部
                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(5000);
                #region  网页的源码
                outerHtml = "";
                OuterHtml();//获取网页源码
                int oh = 0;
                while (oh < 360)
                {
                    oh++;
                    if (outerHtml == "")//没有开始
                    {
                        Thread.Sleep(500);
                    }
                    else if (outerHtml.Contains("没有获取到源码"))//没有百度源码
                    {
                        address = CWebBrowser.Address;//当前网页的网址
                        pageRun();
                        break;
                    }
                    else if (outerHtml.Length > 10)//获取到源码
                    {
                        pageRun();
                        break;
                    }
                }
                #endregion

                rankingManage(outerHtml);
            }
        }


        /// <summary>
        /// 定时结束 下一页刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void PageOver(object sender, EventArgs e)
        {
            //if (CWebBrowser.IsLoading)
            //{
            //    return;
            //}
            // await CWebBrowser.GetSourceAsync();
            #region  网页的源码
            var result = ""; //网页的源码
            outerHtml = "";
            OuterHtml();//获取网页源码
            int oh = 0;
            while (oh < 360)
            {
                oh++;
                if (outerHtml == "")//没有开始
                {
                    Thread.Sleep(500);
                }
                else if (outerHtml.Contains("没有获取到源码"))//没有百度源码
                {
                    address = CWebBrowser.Address;//当前网页的网址
                    result = await CWebBrowser.GetSourceAsync();
                    outerHtml = result.ToString();
                    break;
                }
                else if (outerHtml.Length > 10)//获取到源码
                {
                    result = outerHtml;
                    break;
                }
            }
            #endregion

            if (result != null && result != "")
            {
                Label_Status("判断页面是否加载完成！", Color.Black);

                #region 页面出现异常
                if (!isTarget && result.Length < 30000)
                {
                    if (result == "<html><head></head><body></body></html>")//if (result.Contains("window.location.replace") && result.Contains("<meta content=\"always\" name=\"referrer\">"))
                    {
                        Label_Status("页面出现白屏，开始后退！", Color.Red);
                        CWebBrowser.GetBrowser().GoBack();//后退
                        //Timer.Start();
                        //TimerOver.Start();
                    }
                    return;
                }
                #endregion

                if (LastPage == result || pageIsOver)
                {
                    Label_Status("当前页和上一页源码页面相同！刷新重试...", Color.Black);
                    LastPage = "";
                    Thread.Sleep(2000);
                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                    Timer.Start();
                    TimerOver.Start();
                    return;
                }
                else
                {
                    LastPage = result;
                }
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(result);
                //HtmlNode htmlnode = doc.DocumentNode.SelectSingleNode("//div[@class='c-result result']");//详情页搜索结果
                //if (htmlnode != null)
                //{
                HtmlNode NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav c-flexbox']");
                if (NextPageNode == null)
                {
                    NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav-right']");
                }
                if (NextPageNode != null)
                {
                    #region  显示标题
                    this.Invoke((EventHandler)(delegate
                    {
                        try
                        {
                            textBox_Url.Text = CWebBrowser.Address;//网址
                            address = CWebBrowser.Address;//当前网页的网址
                            Regex titleRegex = new Regex("(?<=<title>).*?(?=</title>)");
                            tabControl1.TabPages[0].Text = titleRegex.Match(result).Value;//标题
                        }
                        catch
                        {
                            tabControl1.TabPages[0].Text = "";
                        }
                    }));
                    #endregion

                    #region 百度安全验证
                    HaveSafe();
                    int hs = 0;
                    while (hs < 240)
                    {
                        hs++;
                        if (HaveBaiduSafe == -1)//没有百度验证
                        {
                            Label_Status("没有百度验证！", Color.Black);
                            HaveBaiduSafe = 0;
                            break;
                        }
                        else if (HaveBaiduSafe == 1)//出现百度验证
                        {
                            HaveBaiduSafe = 0;
                            Label_Status("出现百度安全验证，结束此方法！", Color.Black);
                            return;
                        }
                        else if (HaveBaiduSafe == 0)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    #endregion

                    if (PageOverDic.ContainsKey(address))
                    {
                        Label_Status("PageOverDic包含当前网址！", Color.Black);
                        PageOverDic[address] += 1;
                    }
                    else
                    {
                        PageOverDic.Add(address, 1);
                    }


                    Timer.Stop();
                    TimerOver.Stop();
                    if (pageIsOver)
                    {
                        return;//由页面刷新结束执行之后操作
                    }
                    pageIsOver = true;
                    Label_Status("开始下一页的点击！", Color.Black);
                    PageResult(result.ToString().ToLower(), model_task.task_domain);
                }
            }
        }

        /// <summary>
        /// 判断网页是否是我们网址
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="model_task_task_domain"></param>
        public void PageResult(string Html, string model_task_task_domain)
        {

            #region 判断网页的页数
            int page = 0;
            string address = CWebBrowser.Address;//当前网页的网址
            if (address.Contains("pn="))
            {
                Regex pageRegex = new Regex("(?<=pn=).*?(?=&)");
                page = Convert.ToInt32(pageRegex.Match(address).Value);
                int pageNum = (page / 10) + 1;
                //page = Convert.ToInt32(address.Substring(address.IndexOf("pn=") + 3, 2));
                Label_Status("搜索第" + pageNum + "页加载完成！", Color.Black);
            }
            #endregion

            //随机暂停时间
            Random random = new Random();
            int n = random.Next(5, 10);
            Thread.Sleep(n * 1000);

            #region 展开更多搜索结果
            HaveMoreResult();//js判断是否有更多搜索按钮
            int hmr = 0;
            while (true)
            {
                hmr++;
                if (hmr >= 240)
                {
                    PageOverDic.Remove(address);//后退和刷新删掉本页链接
                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                    Timer.Start();
                    TimerOver.Start();
                    return;
                }
                if (HaveMoveClick == 0)
                {
                    Thread.Sleep(500);
                }
                else if (HaveMoveClick == 1)//有更多搜索结果
                {
                    HaveMoveClick = 0;
                    PageOverDic.Remove(address);//后退和刷新删掉本页链接
                    pageIsOver = false;

                    #region 验证更多搜索结果
                    Label_Status("更多搜索结果，开始点击！", Color.Red);
                    MoreSearch();//更多搜索结果
                    int ms = 0;
                    while (ms < 240)
                    {
                        ms++;
                        if (moreSearchCount >= 10)
                        {
                            Label_Status("连续10次出现更多搜索结果，程序结束！", Color.Green);
                            Thread.Sleep(2000);
                            TaskEnd();//程序结束
                        }
                        if (moreSearchType == 1)//验证通过
                        {
                            moreSearchType = 0;
                            moreSearchCount++;
                            Label_Status("更多搜索验证成功！", Color.Red);
                            Thread.Sleep(3000);
                            Browser_Page(null, null);
                            return;
                        }
                        else if (moreSearchType == -1)//验证失败
                        {
                            moreSearchType = 0;
                            moreSearchCount++;
                            Label_Status("更多搜索结果失败，刷新重试...", Color.Red);
                            Thread.Sleep(2000);
                            CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                            Timer.Start();
                            TimerOver.Start();
                            return;
                        }
                        else if (moreSearchType == 0)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    break;
                    #endregion
                }
                else if (HaveMoveClick == -1)//没有更多搜索结果
                {
                    HaveMoveClick = 0;
                    break;
                }
            }
            #endregion

            int order = 0;//排名
                          //获取网址内容
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(Html);
            HtmlNode htmlnode = doc.DocumentNode.SelectSingleNode("//div[@class='results']");
            if (htmlnode != null)
            {
                string OurUrl = "";//判断自己的网址标记
                string ContentHTML = "";//判断自己的网址标记
                string sjUrl = "";//手机网址
                HtmlAgilityPack.HtmlNodeCollection nodecollection = htmlnode.ChildNodes;
                nodecollection.RemoveAt(0);//删除第一条数据
                for (int i = 0; i < nodecollection.Count; i++)
                {
                    string result = nodecollection[i].OuterHtml.ToLower();
                    if (result.Length > 300)
                    {
                        HtmlAgilityPack.HtmlDocument docAttribute = new HtmlAgilityPack.HtmlDocument();
                        docAttribute.LoadHtml(result);

                        #region 获取网页排名和网址
                        string HtmlUrl = "";//获取手机网页
                        try
                        {
                            HtmlNode urlNode = docAttribute.DocumentNode.SelectSingleNode("//div[@class='c-result result']");
                            if (urlNode != null)
                            {
                                HtmlAttributeCollection attrs = urlNode.Attributes;
                                foreach (var item in attrs)
                                {
                                    if (item.Name == "order")//获取网址的排名
                                    {
                                        string value = item.Value;
                                        if (value != "")
                                        {
                                            order = Convert.ToInt32(value);//名次
                                        }
                                    }
                                    if (item.Name == "data-log")//获取网址的网址
                                    {
                                        string value = item.Value;
                                        if (value != "")
                                        {
                                            if (value.Contains("http"))
                                            {
                                                HtmlUrl = value.Substring(value.LastIndexOf("http")).Replace("'}", "").Replace("'", "").Replace("&quot;", "").Replace("}", "");
                                            }
                                            //HtmlUrl = value.Substring(value.LastIndexOf("':'")).Replace("'}", "").Replace("':'", "").ToLower();//网址
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Label_Status("获取网页网址出现错误：" + ex.Message, Color.Red);
                        }
                        #endregion

                        #region 获取网页根域名
                        string HtmlDomain = "";
                        try
                        {
                            //HtmlNode domainNode = docAttribute.DocumentNode.SelectSingleNode("//span[@class='c-color-gray']");
                            //if (domainNode == null)
                            //{
                            //    domainNode = docAttribute.DocumentNode.SelectSingleNode("//span[@class='c-showurl c-footer-showurl']");
                            //}
                            //if (domainNode != null)
                            //{
                            //    HtmlAgilityPack.HtmlNodeCollection domainChild = domainNode.ChildNodes;
                            //    for (int j = 0; j < domainChild.Count; j++)
                            //    {
                            //        if (domainChild[j].InnerText != "")
                            //        {
                            //            HtmlDomain = domainChild[j].InnerText;
                            //            break;
                            //        }
                            //    }
                            //}
                            if (HtmlUrl != "")
                            {
                                Regex regex = new Regex("(?<=//).*?(?=/)");
                                Match match = regex.Match(HtmlUrl);
                                if (match.Success)
                                {
                                    HtmlDomain = match.Value;
                                }
                                else
                                {
                                    HtmlDomain = HtmlUrl.Substring(HtmlUrl.IndexOf("//") + 2);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Label_Status("获取网页域名出现错误：" + ex.Message, Color.Red);
                        }
                        #endregion

                        #region 排名信息统计
                        if (HtmlUrl != "")
                        {
                            try
                            {
                                sjUrl = HtmlUrl.Replace("http://", "").Replace("https://", "");
                                if (sjUrl.EndsWith("/"))
                                {
                                    sjUrl = sjUrl.Substring(0, sjUrl.LastIndexOf('/'));
                                }
                                foreach (string sjDomain in Library.BaseTable.SjWebsite)
                                {
                                    string sjDomain2 = sjDomain.Trim().ToLower();
                                    if (sjUrl.Equals(sjDomain2))//网址包含基表中的网址
                                    {
                                        int ranking = page + order;//排名
                                        string daowei = "";//是否到位
                                        if (ranking < 9)
                                        {
                                            daowei = "到位";
                                        }
                                        else
                                        {
                                            daowei = "不到位";
                                        }
                                        string strMsg = "";
                                        if (Library.BaseTable.keyWords.Contains(model_task.task_word))//重点词基表是否包含关键词
                                        {
                                            strMsg = "SJ" + "$" + "重点词" + "$" + model_task.task_word + "$" + HtmlDomain + "$" + HtmlUrl + "$" + localIP + "$" + ranking + "$" + IP + "$" + daowei + "$" + formText + "$" + sjDomain2;
                                        }
                                        else if (Library.BaseTable.commonWords.Contains(model_task.task_word))//普通词基表是否包含关键词
                                        {
                                            strMsg = "SJ" + "$" + "普通词" + "$" + model_task.task_word + "$" + HtmlDomain + "$" + HtmlUrl + "$" + localIP + "$" + ranking + "$" + IP + "$" + daowei + "$" + formText + "$" + sjDomain2;
                                        }
                                        else if (Library.BaseTable.BrandWordList.Contains(model_task.task_word))//品牌词基表是否包含关键词
                                        {
                                            strMsg = "SJ" + "$" + "品牌词" + "$" + model_task.task_word + "$" + HtmlDomain + "$" + HtmlUrl + "$" + localIP + "$" + ranking + "$" + IP + "$" + daowei + "$" + formText + "$" + sjDomain2;
                                        }
                                        else if (Library.BaseTable.BiddingWordList.Contains(model_task.task_word))//竞价词基表是否包含关键词
                                        {
                                            strMsg = "SJ" + "$" + "竞价词" + "$" + model_task.task_word + "$" + HtmlDomain + "$" + HtmlUrl + "$" + localIP + "$" + ranking + "$" + IP + "$" + daowei + "$" + formText + "$" + sjDomain2;
                                        }
                                        wordsList.Add(strMsg);
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Label_Status("排名出现错误：" + ex.Message, Color.Red);
                            }
                        }
                        #endregion

                        #region 没有带*的域名
                        if (!model_task_task_domain.Contains("*"))
                        {
                            if (HtmlUrl != "" && sjUrl != "")
                            {
                                string task_domain = model_task_task_domain;
                                if (task_domain.EndsWith("/"))
                                {
                                    task_domain = task_domain.Substring(0, task_domain.LastIndexOf('/'));
                                }
                                if (sjUrl.Equals(task_domain))
                                {
                                    if (domainEquals(sjUrl, task_domain))
                                    {
                                        #region 正则获取搜索结果
                                        try
                                        {
                                            Regex r = new Regex("(?=搜索结果).*?(?<=条.标题)");
                                            Match m = r.Match(result);
                                            OurUrl = m.Value.ToString(); ;
                                        }
                                        catch (Exception ex)
                                        {
                                            Label_Status("搜索结果正则出现错误：" + ex.Message, Color.Red);
                                        }
                                        #endregion

                                        #region 获取网页内容文本
                                        try
                                        {
                                            //HtmlNode ContentNode = docAttribute.DocumentNode.SelectSingleNode("//a[@href='c-img-wrapper']");
                                            //if (ContentNode == null)
                                            //{
                                            HtmlNode ContentNode = docAttribute.DocumentNode.SelectSingleNode("//div[@class='c-row']");
                                            //}
                                            if (ContentNode == null)
                                            {
                                                ContentNode = docAttribute.DocumentNode.SelectSingleNode("//div[@class='c-abstract c-row']");
                                            }
                                            if (ContentNode != null)
                                            {
                                                HtmlAgilityPack.HtmlNodeCollection ContentChild = ContentNode.ChildNodes;
                                                for (int j = 0; j < ContentChild.Count; j++)
                                                {
                                                    if (ContentChild[j].InnerText != "")
                                                    {
                                                        ContentHTML = ContentChild[j].InnerText;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Label_Status("获取网页内容文本出现错误：" + ex.Message, Color.Red);
                                        }
                                        #endregion

                                        if (OurUrl == "" && ContentHTML == "")
                                        {
                                            OurUrl = HtmlUrl;
                                        }
                                        Label_Status("SJ关键词：" + model_task.task_id + " " + model_task.task_word + " 目标域名：" + model_task.task_domain + " 当前网址:" + HtmlUrl + " 排名：" + (page + order), Color.Green);
                                        break;
                                    }
                                }
                                else
                                {
                                    Label_Status("SJ关键词：" + model_task.task_id + " " + model_task.task_word + " 目标域名：" + model_task.task_domain + " 当前网址:" + HtmlUrl + " 排名：" + (page + order), Color.Black);
                                    continue;
                                }
                            }
                            else
                            {
                                Label_Status("没有获取到网址！", Color.Red);
                                continue;
                            }
                        }
                        else
                        {
                            Label_Status("关键词【" + model_task.task_word + "】的物料【" + model_task.task_domain + "】不符合点击标准！", Color.Red);
                            model_task_Log.task_log_sort = 101;
                            Thread.Sleep(10000);
                            TaskEnd();//程序结束
                        }
                        #endregion
                    }
                    //Thread.Sleep(500);
                }
                //Thread.Sleep(2000);
                if (OurUrl == "" && ContentHTML == "")
                {
                    if (page >= 90)//address.Contains("pn=90&")
                    {
                        model_task_Log.task_log_sort = 101;
                        TaskEnd();//程序结束
                    }
                    else
                    {
                        #region 点击下一页
                        HtmlNode NextPageNode;
                        if (page == 0)
                        {
                            NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav c-flexbox']");
                        }
                        else
                        {
                            NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav-right']");
                        }
                        if (NextPageNode == null)
                        {
                            #region 没有获取到下一页的链接
                            if (repeadCount >= 2)
                            {
                                Label_Status("关键词【" + model_task.task_word + "】没有下一页的链接，程序关闭！", Color.Red);
                                model_task_Log.task_log_sort = 101;
                                Thread.Sleep(2000);
                                TaskEnd();//程序结束
                            }
                            PageOverDic.Remove(address);//后退和刷新删掉本页链接
                            Label_Status("没有获取到下一页的链接，重新刷新页面！", Color.Red);
                            CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                            repeadCount++;
                            Timer.Start();
                            TimerOver.Start();
                            pageIsOver = false;
                            return;
                            #endregion
                        }
                        else
                        {
                            if (NextPageNode.InnerText == "")
                            {
                                #region 没有获取到下一页的链接
                                if (repeadCount >= 2)
                                {
                                    Label_Status("关键词【" + model_task.task_word + "】没有下一页的链接，程序关闭！", Color.Red);
                                    model_task_Log.task_log_sort = 101;
                                    Thread.Sleep(2000);
                                    TaskEnd();//程序结束
                                }
                                Label_Status("没有获取到下一页的链接，重新刷新页面！", Color.Red);
                                PageOverDic.Remove(address);//后退和刷新删掉本页链接
                                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                                repeadCount++;
                                Timer.Start();
                                TimerOver.Start();
                                pageIsOver = false;
                                return;
                                #endregion
                            }
                            else
                            {
                                //滚动到底部
                                for (int i = 1; i < 50; i++)
                                {
                                    CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, " + (i * 50) + ");"); //滚动
                                    Thread.Sleep(40);
                                }
                                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight);");//滚动到底部
                                repeadCount = 0;
                                Label_Status("搜索第" + (page / 10 + 2) + "页加载中...", Color.Black);

                                #region  点击下一页
                                NextClick();
                                int nc = 0;
                                while (nc < 240)
                                {
                                    nc++;
                                    if (HaveNextPage == 1)//验证通过
                                    {
                                        HaveNextPage = 0;
                                        break;
                                    }
                                    else if (HaveNextPage == -1)//验证失败
                                    {
                                        HaveNextPage = 0;
                                        NextReloadCount++;
                                        PageOverDic.Remove(address);//后退和刷新删掉本页链接
                                        if (NextReloadCount >= 3)
                                        {
                                            Label_Status("连续3次没有获取到下一页的链接，程序关闭！", Color.Red);
                                            TaskEnd();//程序结束
                                        }
                                        Label_Status("没有获取到下一页的链接，刷新重试！", Color.Red);
                                        CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                                        Timer.Start();
                                        TimerOver.Start();
                                        pageIsOver = false;
                                        return;
                                    }
                                    else if (HaveNextPage == 0)
                                    {
                                        Thread.Sleep(500);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region 点击目标页
                    model_task_Log.task_log_sort = page + order;
                    isTarget = true;//点击到目标页
                    if (order > 3)
                    {
                        for (int i = 1; i < order * 10; i++)
                        {
                            CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, " + (i * 20) + ");"); //滚动
                            Thread.Sleep(40);
                        }
                    }
                    Label_Status("点击目标页面加载中...", Color.Green);

                    //点击目标网址
                    if (OurUrl != "")
                    {
                        Label_Status("使用方法1点击目标页..." + OurUrl, Color.Green);
                        LinkClick(OurUrl);
                    }
                    else if (OurUrl == "")
                    {
                        Label_Status("使用方法2点击目标页..." + model_task_task_domain, Color.Green);
                        LinkClick(model_task_task_domain);
                    }
                    int lc = 0;
                    while (true)
                    {
                        lc++;
                        if (lc >= 240)
                        {
                            //微信提醒
                            HttpGet.HttpGET(wechatApi + "SJ端：账号：" + user_email + "点击ID：" + model_task.task_id + "，关键词：" + model_task.task_word + "，域名：" + model_task.task_domain + "时没有点击到目标页，请尽快处理！");
                            TaskEnd();//程序结束
                            break;
                        }
                        if (TargetType == 1)//验证通过
                        {
                            TargetType = 0;
                            break;
                        }
                        else if (TargetType == -1)//验证失败
                        {
                            TargetType = 0;
                            TargetCount++;
                            if (TargetCount >= 3)
                            {
                                Label_Status("连续3次没有获取到目标页的链接，程序关闭！", Color.Red);
                                HttpGet.HttpGET(wechatApi + "SJ端：账号：" + user_email + "点击ID：" + model_task.task_id + "，关键词：" + model_task.task_word + "，域名：" + model_task.task_domain + "连续3次没有获取到目标页的链接！");
                                TaskEnd();//程序结束
                            }
                            PageOverDic.Remove(address);//后退和刷新删掉本页链接
                            Label_Status("没有获取到目标页的链接，刷新重试！", Color.Red);
                            CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");//刷新页面
                            Timer.Start();
                            TimerOver.Start();
                            pageIsOver = false;
                            return;
                        }
                        else if (TargetType == 0)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                Label_Status("获取页面标签错误，需要联系管理员修改！", Color.Red);
                Thread.Sleep(5000);
                TaskEnd();//程序结束
            }
        }






        /// <summary>
        /// 目标页加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TargetOver(object sender, EventArgs e)
        {
            if (targetIsOver)
            {
                return;//由页面刷新结束执行之后操作
            }
            targetIsOver = true;
            TimerTarget.Stop();
            Label_Status("目标页加载完成！", Color.Green);
            Thread thread = new Thread(delegate ()
            {
                Thread.Sleep(10000);
                TaskEnd();//程序结束
            });
            thread.Start();
        }



        #region 鼠标移动点击
        #region 鼠标属性
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern IntPtr SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_WHEEL = 0x800;
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);
        const int URLMON_OPTION_USERAGENT = 0x10000001;
        #endregion

        /// <summary>
        /// 获取网页源码
        /// </summary>
        public void OuterHtml()
        {
            try
            {
                String script =
              @"(function() {
                    return document.documentElement.outerHTML;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        outerHtml = response.Result.ToString();
                    }
                    else
                    {
                        outerHtml = "没有获取到源码！";
                    }
                });
            }
            catch (Exception ex)
            {
                //Label_Status("获取源码错误：" + ex.Message, Color.Red);
                outerHtml = "没有获取到源码！";
            }
        }

        /// <summary>
        ///  点击登录按钮
        /// </summary>
        public void ButtonClick()
        {
            try
            {
                String script =
                @"(function() {
                    var classHtml = document.getElementsByClassName('loginBtn');

                     var pos = classHtml[0].getBoundingClientRect();
                        var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                            return x + ',' + y;

                 })()";


                //var pos = tags.getBoundingClientRect();
                //var x = pos.left + pos.width / 2;
                //var y = pos.top + pos.height / 2 + 35;
                //return x + ',' + y;
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 50;
                            //移动鼠标点击
                            int count = 100;
                            while (count != 0)
                            {
                                Thread.Sleep(15);
                                int stepx = (xx - Cursor.Position.X) / count;
                                int stepy = (yy - Cursor.Position.Y) / count;


                                count--;
                                if (count != 0)
                                    SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                            }
                            SetCursorPos(xx, yy);
                            //Thread.Sleep(100);
                            //Thread.Sleep(2000);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                            Thread.Sleep(50);
                            mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                            Timer.Start();
                            TimerOver.Start();
                            pageIsOver = false;
                            ButtonType = 1;
                        }
                        else
                        {
                            ButtonType = -1;
                        }
                    }
                    else
                    {
                        ButtonType = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("获取到搜索按钮的链接出错：" + ex.Message, Color.Red);
                ButtonType = -1;
            }
        }


        /// <summary>
        ///  点击搜索框
        /// </summary>
        public void InputClick()
        {
            try
            {
                String script =
                @"(function() {
                    var classHtml = document.getElementsByClassName('webNavSearch');

                     var pos = classHtml[0].getBoundingClientRect();
                        var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                            return x + ',' + y;

                 })()";


                //var pos = tags.getBoundingClientRect();
                //var x = pos.left + pos.width / 2;
                //var y = pos.top + pos.height / 2 + 35;
                //return x + ',' + y;
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 50;
                            //移动鼠标点击
                            int count = 100;
                            while (count != 0)
                            {
                                Thread.Sleep(15);
                                int stepx = (xx - Cursor.Position.X) / count;
                                int stepy = (yy - Cursor.Position.Y) / count;


                                count--;
                                if (count != 0)
                                    SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                            }
                            SetCursorPos(xx, yy);
                            //Thread.Sleep(100);
                            //Thread.Sleep(2000);
                            mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                            Thread.Sleep(50);
                            mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                            Timer.Start();
                            TimerOver.Start();
                            pageIsOver = false;
                            ButtonType = 1;
                        }
                        else
                        {
                            ButtonType = -1;
                        }
                    }
                    else
                    {
                        ButtonType = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("获取到搜索框的链接出错：" + ex.Message, Color.Red);
                ButtonType = -1;
            }
        }



        /// <summary>
        ///  目标页的点击1
        /// </summary>
        public void LinkClick2(string curl)
        {
            try
            {
                String script =
             @"(function() {
               var result = '';
                            var classHtml = document.getElementsByClassName('c-row');
                            if (classHtml == null)
                            {
                                classHtml = document.getElementsByClassName('c-abstract c-row');
                            }
                            for (var i = 0, j = classHtml.length; i < j; i++)
                            {
                                var Html = classHtml[i].innerText;
                                if (Html.search('" + curl + @"') != -1)
                                {
                                            var pos = classHtml[i].getBoundingClientRect();
                                            var x = pos.left + pos.width / 2;
                                            var y = pos.top + pos.height / 2;
                                            return x + ',' + y;
                                }
                            }
                            return result;
             })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {

                    //var hl = '';
                    //for (var g = 0, d = 2; g < d; g++)
                    //{
                    //    if (g == 0)
                    //    {

                    //    }
                    //    if (g == 1)
                    //    {
                    //var pos = classHtml[i].getBoundingClientRect();
                    //var y = pos.top;
                    //hl = y;
                    //window.scrollBy(0, y);
                    //}
                    //  }
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);

                            int xx = 100;
                            int yy = (int)y1 + 30;

                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶
                                                             //移动鼠标点击
                                                             //int count = 100;
                                                             //while (count != 0)
                                                             //{
                                                             //    Thread.Sleep(15);
                                                             //    int stepx = (xx - Cursor.Position.X) / count;
                                                             //    int stepy = (yy - Cursor.Position.Y) / count;


                                //    count--;
                                //    if (count != 0)
                                //        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                //}
                                SetCursorPos(xx, yy);
                                Thread.Sleep(100);
                                Thread.Sleep(2000);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                Thread.Sleep(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                TimerTarget.Start();
                            }
                            else
                            {
                                Label_Status("没有获取目标页的链接", Color.Red);
                                Thread.Sleep(5000);
                                TaskEnd();//程序结束
                            }
                        }
                        else
                        {
                            Label_Status("没有获取目标页的链接", Color.Red);
                            Thread.Sleep(5000);
                            TaskEnd();//程序结束
                        }
                    }
                    else
                    {
                        Label_Status("没有获取目标页的链接", Color.Red);
                        Thread.Sleep(5000);
                        TaskEnd();//程序结束
                    }

                });
            }
            catch (Exception ex)
            {
                Label_Status("获取目标页的链接出错：" + ex.Message, Color.Red);
                TaskEnd();//程序结束
            }
        }
        /// <summary>
        ///  目标页的点击2
        /// </summary>
        public void LinkClick(string curl)
        {
            try
            {
                String script =
             @"(function() {
                var result = '';
                    var classHtml = document.getElementsByClassName('c-result result');
                     for (var i = 0, j = classHtml.length; i < j; i++)
                    {
                        var Html = classHtml[i].outerHTML.toLowerCase();
                        if (Html.search('" + curl + @"') != -1) 
                        {

                            var hl = '';
                            for (var g = 0, d = 2; g < d; g++)
                            {
                                if (g == 0)
                                {
                                    var pos = classHtml[i].getBoundingClientRect();
                                    var y = pos.top;
                                    hl = y;
                                    window.scrollBy(0, y - 100);
                                }
                                if (g == 1)
                                {
                                    var pos = classHtml[i].getBoundingClientRect();
                                    var x = pos.left + pos.width / 2;
                                    var y = pos.top + pos.height / 2;
                                    return x + ',' + y;
                                }
                            }
                        }
                        else
                        {
                            result = Html;
                        }
                    }
                    return result;
             })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    //if (Html.indexOf('" + model_task_task_domain + @"') != -1 || Html.search('" + model_task_task_domain + @"') != -1)
                    //{
                    //}

                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            if (result.ToString().Length > 500)
                            {
                                Label_Status("网页源码：" + result.ToString(), Color.Red);
                            }
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 60;
                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶

                                //移动鼠标点击
                                int count = 100;
                                while (count != 0)
                                {
                                    Thread.Sleep(15);
                                    int stepx = (xx - Cursor.Position.X) / count;
                                    int stepy = (yy - Cursor.Position.Y) / count;

                                    count--;
                                    if (count != 0)
                                        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                }
                                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, yy);"); //滚动
                                SetCursorPos(xx, yy);
                                //Thread.Sleep(100);
                                //Thread.Sleep(2000);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                Thread.Sleep(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                TimerTarget.Start();
                                TargetType = 1;
                            }
                            else
                            {
                                TargetType = -1;
                            }
                        }
                        else
                        {
                            TargetType = -1;
                        }
                    }
                    else
                    {
                        TargetType = -1;
                    }

                });
            }
            catch (Exception ex)
            {
                Label_Status("获取目标页的链接出错：" + ex.Message, Color.Red);
                TargetType = -1;
            }
        }



        /// <summary>
        ///  详情页点击
        /// </summary>
        public void DetailsPageClick(string divName)
        {
            try
            {
                String script =
             @"(function() {
                        var result =  document.getElementById('" + divName + @"');

                            var hl = '';
                            for (var g = 0, d = 2; g < d; g++)
                            {
                                if (g == 0)
                                {
                                    var pos = result.getBoundingClientRect();
                                    var y = pos.top;
                                    hl = y;
                                    window.scrollBy(0, y - 100);
                                }
                                if (g == 1)
                                {
                                    var pos = result.getBoundingClientRect();
                                    var x = pos.left + pos.width / 2;
                                    var y = pos.top + pos.height / 2;
                                    return x + ',' + y;
                                }
                            }

                     return result;
             })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    //if (Html.indexOf('" + model_task_task_domain + @"') != -1 || Html.search('" + model_task_task_domain + @"') != -1)
                    //{
                    //}

                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            if (result.ToString().Length > 500)
                            {
                                Label_Status("网页源码：" + result.ToString(), Color.Red);
                                return;
                            }
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 60;
                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶

                                //移动鼠标点击
                                int count = 100;
                                while (count != 0)
                                {
                                    Thread.Sleep(15);
                                    int stepx = (xx - Cursor.Position.X) / count;
                                    int stepy = (yy - Cursor.Position.Y) / count;

                                    count--;
                                    if (count != 0)
                                        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                }
                                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, yy);"); //滚动
                                SetCursorPos(xx, yy);
                                //Thread.Sleep(100);
                                //Thread.Sleep(2000);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                Thread.Sleep(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                TimerTarget.Start();
                                TargetType = 1;
                            }
                            else
                            {
                                TargetType = -1;
                            }
                        }
                        else
                        {
                            TargetType = -1;
                        }
                    }
                    else
                    {
                        TargetType = -1;
                    }

                });
            }
            catch (Exception ex)
            {
                Label_Status("获取目标页的链接出错：" + ex.Message, Color.Red);
                TargetType = -1;
            }
        }

        /// <summary>
        /// 下一页的点击
        /// </summary>
        public void NextClick()
        {
            try
            {
                String script =
               @"(function() {
                    var result = '';
                    var classHtml = document.getElementsByTagName('a');
                    for (var i = 0, j = classHtml.length; i < j; i++)
                    {
                        if (classHtml[i].className.search('new-nextpage') != -1)
                        {
                            window.scrollTo(0, document.body.scrollHeight); /*滚动到底部*/
                            var pos = classHtml[i].getBoundingClientRect();
                            var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                       
                            return x + ',' + y;
                        }
                    }
                    return result;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    // window.scrollBy(0,y);
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 40;

                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶
                                                             //Thread.Sleep(2000);
                                                             //移动鼠标点击
                                int count = 100;
                                while (count != 0)
                                {
                                    Thread.Sleep(15);
                                    int stepx = (xx - Cursor.Position.X) / count;
                                    int stepy = (yy - Cursor.Position.Y) / count;


                                    count--;
                                    if (count != 0)
                                        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                }
                                //Thread.Sleep(2000);
                                CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight);");//滚动到底部
                                SetCursorPos(xx, yy);
                                //Thread.Sleep(100);
                                //Thread.Sleep(2000);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                Thread.Sleep(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                Timer.Start();
                                TimerOver.Start();
                                pageIsOver = false;
                                NextPageUrl();//下一页提交前网址
                                HaveNextPage = 1;
                            }
                            else
                            {
                                HaveNextPage = -1;
                            }
                        }
                        else
                        {
                            HaveNextPage = -1;
                        }
                    }
                    else
                    {
                        HaveNextPage = -1;
                    }

                });
            }
            catch (Exception ex)
            {
                Label_Status("获取到下一页的链接出错：" + ex.Message, Color.Red);
                HaveNextPage = -1;
            }
        }

        /// <summary>
        /// 判断是否有更多搜索按钮
        /// </summary>
        public void HaveMoreResult()
        {
            try
            {
                String script =
               @"(function() {
                    var result = '';
                    var classHtml = document.getElementsByTagName('div');
                    for (var i = 0, j = classHtml.length; i < j; i++)
                    {
                        if (classHtml[i].className.search('hint-fold-results-box') != -1)
                        {
                            window.scrollTo(0, document.body.scrollHeight); /*滚动到底部*/
                            var pos = classHtml[i].getBoundingClientRect();
                            var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                            return x + ',' + y;
                        }
                    }
                    return result;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 40;

                            if (xx > 1 && yy > 1)
                            {
                                HaveMoveClick = 1;
                            }
                            else
                            {
                                HaveMoveClick = -1;
                            }
                        }
                        else
                        {
                            HaveMoveClick = -1;
                        }
                    }
                    else
                    {
                        HaveMoveClick = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("判断是否有更多搜索按钮出错：" + ex.Message, Color.Red);
                HaveMoveClick = -1;
            }
        }

        /// <summary>
        /// 判断是否有百度安全验证
        /// </summary>
        public void HaveSafe()
        {
            try
            {
                String script =
               @"(function() {
                    return document.documentElement.outerHTML;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {

                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString().Contains("<title>百度安全验证</title>"))
                        {
                            Label_Status("页面有百度验证！", Color.Red);
                            HaveBaiduSafe = 1;
                        }
                        else
                        {
                            baduSafeHtml = result.ToString();
                            HaveBaiduSafe = -1;
                        }
                    }
                    else
                    {
                        HaveBaiduSafe = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("验证百度安全验证出错：" + ex.Message, Color.Red);
                HaveBaiduSafe = -1;
            }
        }

        /// <summary>
        /// 验证百度安全验证
        /// </summary>
        public void SafeVerify()
        {
            try
            {
                String script =
               @"(function() {
                    var result = '';
                    var classHtml = document.getElementsByTagName('div');
                    for (var i = 0, j = classHtml.length; i < j; i++)
                    {
                        if (classHtml[i].className.search('vcode-slide-button') != -1)
                        {
                            var pos = classHtml[i].getBoundingClientRect();
                            var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                            return x + ',' + y;
                        }
                    }
                    return result;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {

                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 40;

                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶
                                SetCursorPos(xx, yy);  //移动鼠标点击
                                                       //Thread.Sleep(100);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                //Thread.Sleep(50);
                                //Thread.Sleep(2000);

                                int count = 100;
                                while (count != 0)
                                {
                                    Thread.Sleep(15);
                                    int stepx = (xx + 250 - Cursor.Position.X) / count;
                                    int stepy = (yy - Cursor.Position.Y) / count;

                                    count--;
                                    if (count != 0)
                                        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                }
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                BaiduSafeSuc = 1;
                            }
                            else
                            {
                                BaiduSafeSuc = -1;
                            }
                        }
                        else
                        {
                            BaiduSafeSuc = -1;
                        }
                    }
                    else
                    {
                        BaiduSafeSuc = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("获取验证码出错：" + ex.Message, Color.Red);
                BaiduSafeSuc = -1;
            }
        }

        /// <summary>
        /// 点击更多搜索结果
        /// </summary>
        public void MoreSearch()
        {
            try
            {
                String script =
               @"(function() {
                    var result = '';
                    var classHtml = document.getElementsByTagName('div');
                    for (var i = 0, j = classHtml.length; i < j; i++)
                    {
                        if (classHtml[i].className.search('unfold') != -1)
                        {
                            var pos = classHtml[i].getBoundingClientRect();
                            var x = pos.left + pos.width / 2;
                            var y = pos.top + pos.height / 2 + 10;
                            return x + ',' + y;
                        }
                    }
                    return result;
                 })()";
                CWebBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {

                    var response = x.Result;
                    if (response.Success && response.Result != null)
                    {
                        var result = response.Result;
                        if (result.ToString() != "")
                        {
                            double x1 = double.Parse(result.ToString().Split(',')[0]);
                            double y1 = double.Parse(result.ToString().Split(',')[1]);
                            int xx = (int)x1;
                            int yy = (int)y1 + 60;

                            if (xx > 1 && yy > 1)
                            {
                                SetForegroundWindow(Handle1);//页面置顶
                                int count = 100;
                                while (count != 0)
                                {
                                    Thread.Sleep(15);
                                    int stepx = (xx - Cursor.Position.X) / count;
                                    int stepy = (yy - Cursor.Position.Y) / count;

                                    count--;
                                    if (count != 0)
                                        SetCursorPos(Cursor.Position.X + stepx, Cursor.Position.Y + stepy);
                                }
                                SetCursorPos(xx, yy);  //移动鼠标点击
                                                       //Thread.Sleep(100);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, xx, yy, 0, 0);
                                Thread.Sleep(50);
                                mouse_event(MOUSEEVENTF_LEFTUP, xx, yy, 0, 0);
                                moreSearchType = 1;
                            }
                            else
                            {
                                moreSearchType = -1;
                            }
                        }
                        else
                        {
                            moreSearchType = -1;
                        }
                    }
                    else
                    {
                        moreSearchType = -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Label_Status("点击更多搜索出错：" + ex.Message, Color.Red);
                moreSearchType = -1;
            }
        }
        #endregion

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            this.Invoke((EventHandler)(delegate
            {
                if (e.Frame.IsMain)
                {
                    textBox_Url.Text = e.Url;//提交前网址
                }
            }));
        }

        /// <summary>
        /// 下一页提交前网址
        /// </summary>
        async void NextPageUrl()
        {
            var result = await CWebBrowser.GetSourceAsync();//网页的源码
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(result);

            HtmlNode NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav c-flexbox']");
            if (NextPageNode == null)
            {
                NextPageNode = doc.DocumentNode.SelectSingleNode("//div[@class='new-pagenav-right']");
            }
            if (NextPageNode != null)
            {
                //HtmlNode htmlnode = doc.DocumentNode.SelectSingleNode("//a[@class='new-nextpage']");//详情页搜索结果 new-nextpage-only
                HtmlAttributeCollection attrs = NextPageNode.Attributes;
                foreach (var item in attrs)
                {
                    if (item.Name == "href")//获取网址的排名
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            textBox_Url.Text = item.Value.Replace("amp;", ""); ;//提交前网址
                        }));
                    }
                }
            }
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="Str"></param>
        public void Label_Status(string Str, Color c)
        {
            try
            {
                Invoke(new Action(delegate
                {
                    toolLabel.Text = Str;
                    toolLabel.ForeColor = c;
                }));
                Library.Log.LogSave(pathNow, Str);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool domainEquals(string str1, string str2)
        {
            try
            {
                if (collectDomain(str1).Equals(collectDomain(str2)))//判断，采集到的根域名的值是否相等
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
        public static string collectDomain(string str)
        {
            if (str.Contains("http"))
            {
                Regex r = new Regex("(?<=://).*?(?=/)");
                Match m = r.Match(str);
                str = m.ToString();
            }
            if (str.Contains("*"))
                str = str.Split(new char[] { '*' })[0];//返回值为截取到的根域名的值
            if (str.Contains("/"))
                str = str.Split(new char[] { '/' })[0];//返回值为截取到的根域名的值

            return str;
        }

        /// <summary>
        /// 程序结束
        /// </summary>
        public void TaskEnd()
        {
            if (wordsList.Count > 0)
            {
                Label_Status("开始上传排名数据！", Color.Black);
                //上传排名任务
                UploadInterface.BatchUpload(wordsList, user_email, "words");
                wordsList.Clear();
            }
            Label_Status("上传任务！", Color.Black);
            //File.WriteAllText(TaskPath, model_task.task_id + "|" + model_task.task_word + "|" + model_task.task_domain + "|" + model_task_Log.task_log_sort + "|" + localIP + "|" + IP + "|" + formText + "|" + user_email, Encoding.UTF8);

            File.WriteAllLines(TaskPath, new string[] { phoneUser, phonePwd, model_task.task_word });

            Label_Status("程序结束！", Color.Black);
            Thread.Sleep(2000);
            Process.GetCurrentProcess().Kill();
        }
        /// <summary>
        /// 网页刷新
        /// </summary>
        public void refresh(object sender, EventArgs e)
        {
            timeOutCount++;
            if (timeOutCount > 10)
            {
                Label_Status("网页超时次数超过10次，程序关闭！", Color.Red);
                TaskEnd();
            }
            Label_Status("网页打开超时，重新刷新页面！", Color.Red);

            PageOverDic.Remove(address);//刷新删除当前网址
                                        //CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");
            CWebBrowser.GetBrowser().Reload();//刷新
        }
        /// <summary>
        /// 程序卡死重启
        /// </summary>
        public void AppClose(object sender, EventArgs e)
        {
            Label_Status("程序超时关闭！", Color.Black);
            Process.GetCurrentProcess().Kill();
        }

        //后退
        private void btn_back_Click(object sender, EventArgs e)
        {
            //CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("javascript:history.go(-1);");
            string addressLast = "";
            foreach (KeyValuePair<string, int> item in PageOverDic)
            {
                addressLast = item.Key.ToString();
            }
            PageOverDic.Remove(addressLast);//后退删掉上一页链接
            CWebBrowser.GetBrowser().GoBack();//后退
        }
        //刷新
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("window.location.reload();");
            PageOverDic.Remove(address);//刷新删除当前网址
            CWebBrowser.GetBrowser().Reload();//刷新
        }
        //前进
        private void btn_go_Click(object sender, EventArgs e)
        {
            //CWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("javascript:history.go(1);");
            CWebBrowser.GetBrowser().GoForward();//前进
        }
        //程序关闭
        private void CefSharp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cef.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

    }





}
