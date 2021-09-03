using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Library
{
    public class UploadInterface
    {
        public static List<string> errorUploadList = new List<string>();//网址和根域名不相同集合
        public static List<string> sureList = new List<string>();//网址和根域名不相同集合

        public static string RankingLog = Directory.GetCurrentDirectory() + "\\运行日志\\排名上传日志.txt";
        /// <summary>
        /// 接口登陆用接口
        /// </summary>
        /// <param name="info"></param>
        public static void InterfaceLogin(Model.tb_indentInfo info)
        {
            try
            {
                info.User = "赵灵燕";
                info.Pwd = "116135198081176161045203206216121105053233162036133244073202027218079222200071182219052172105125013144114014073173159053208210171219088005204166";
                info.Rzm = "c1c1dc6a2e7d562798966c6e2fcc31e7";
                info.Plan = 1;
                info.Timeout = 15;
                info.Action = 1;
            }
            catch
            {
                //frmMain.LogError("登陆信息错误【" + ex.Message + "】", "系统");
            }
        }

        /// <summary>
        /// 批量上传关键词排名情况 
        /// </summary>
        /// <param name="keywordresTxt">传递的数据</param>
        /// <param name="key">传递值的类型</param>
        public static void BatchUpload(List<string> keywordresTxt, string user_email, string key)
        {
            try
            {
                string uploadMessage = "";
                Model.tb_indentInfo info = new Model.tb_indentInfo();
                InterfaceLogin(info);  //实例化认证信息对象

                List<Model.tb_youhuaci> keywordresList = new List<Model.tb_youhuaci>();

                for (int i = 0; i < keywordresTxt.Count; i++)
                {
                    string[] keywordresArr = keywordresTxt[i].Split('$');

                    if (keywordresArr.Length != 11)//检测读取的数据格式是否正常
                    {
                        uploadMessage += "测试文本第【" + (i + 1) + "】行数据不规范\r\n";
                    }
                    else
                    {
                        //实例化关键词查询结果对象
                        Model.tb_youhuaci keywordres = new Model.tb_youhuaci();
                        keywordres.ClientLogUser = user_email;
                        keywordres.ClientVer = keywordresArr[9];//挂机端程序版本号 ;
                        keywordres.Createtime = DateTime.Now;
                        keywordres.Domain = keywordresArr[3];//查询域名 ;
                        keywordres.InternetIp = keywordresArr[7];//外网ip ;
                        keywordres.Ip = keywordresArr[5];//内网ip ;
                        keywordres.Rank = Convert.ToInt32(keywordresArr[6]);//查询排名情况
                        keywordres.RankRange = keywordresArr[8];//到位情况
                        keywordres.Title = keywordresArr[2];//关键词
                        keywordres.Url = keywordresArr[4];//网址
                        keywordres.RankType = keywordresArr[0];
                        keywordres.KeywordType = keywordresArr[1];
                        keywordres.TitleMatter = keywordresArr[10];//标题物料                           
                        keywordres.Content = "<br>关键词：【" + keywordres.Title + "】最新排名情况【" + keywordres.Title + "|" + keywordres.Url + "|" + keywordres.Rank + "】,更新时间【" + keywordres.Createtime + "】，来自于挂机端【内网ip：" + keywordres.Ip + "|外网ip：" + keywordres.InternetIp + "|挂机端版本：" + keywordres.ClientVer + "】.";
                        keywordresList.Add(keywordres);
                        uploadMessage += keywordresArr[0] + "|" + keywordresArr[1] + "|" + keywordresArr[2] + "|" + keywordresArr[3] + "|" + keywordresArr[4] + "|" + keywordresArr[5] + "|" + keywordresArr[6] + "|" + keywordresArr[7] + "|" + keywordresArr[8] + "|" + keywordresArr[9] + "|" + keywordresArr[10] + "\r\n";
                    }
                }
                if (keywordresList.Count > 0)
                {
                    //将需要上传的数据保存到dictionary字典对象中
                    Dictionary<int, object> dataDic = new Dictionary<int, object>();
                    dataDic.Add(1, info);
                    dataDic.Add(2, keywordresList);

                    //将所有数据加入到字典容器中
                    Dictionary<int, object> dic = new Dictionary<int, object>();
                    dic.Add(1, 1);
                    dic.Add(2, dataDic);

                    //开始上传数据
                    Dictionary<int, object> resDic = uploadConnect(dic, key);

                    for (int i = 0; i < (resDic.Values.Count * 10); i++)
                    {
                        if (resDic.ContainsKey(i))
                        {
                            uploadMessage += "关键词排名结果:" + resDic[i].ToString() + "\r\n";
                        }
                    }
                    Log.LogSave(RankingLog, uploadMessage);
                }
            }
            catch (Exception ex)
            {
                Log.LogSave(RankingLog, "关键词排名上传时出错【" + ex.Message + "】");
            }
        }


        /// <summary>
        /// 连接接口
        /// </summary>
        /// <param name="dict">传递的数据</param>
        /// <param name="keys">传递值的类型</param>
        /// <returns></returns>
        public static Dictionary<int, object> uploadConnect(Dictionary<int, Object> dict, string keys)
        {
            Dictionary<int, Object> resultDic = new Dictionary<int, Object>();//实例化字典方法
            try
            {
                Library.HttpHelper http = new Library.HttpHelper();
                Library.HttpItem item = new Library.HttpItem();

                if (keys == "words")//关键词排名信息上传后台
                    item.URL = "http://42.51.204.148:89/API/handler1.ashx";
                //item.URL = "http://localhost:59463/API/handler1.ashx";


                //item.URL = "http://192.168.1.242:59463/API/handler1.ashx";
                //else if (keys == "bank")//关键词排名信息上传后台
                //    item.URL = "http://42.51.204.148:89/API/ask.ashx";
                //else if (keys == "noclick")//关键词排名信息上传后台
                //    item.URL = "http://42.51.204.148:89/API/weidianji.ashx";
                //else if (keys == "err")//错误排名信息上传后台
                //    item.URL = "http://114.242.22.122:58657/API/handler.ashx";
                //else if (keys == "upload")//上传积分时出现死锁
                //    item.URL = "http://114.242.22.122:8876/API/handler.ashx";
                //else if (keys == "PCJB")//pc网址不在PC基表中
                //    item.URL = "http://114.242.22.122:40611/API/handler.ashx";


                item.ResultType = Library.ResultType.Byte;
                item.Method = "POST";
                item.PostDataType = Library.PostDataType.Byte;
                item.PostdataByte = Library.DEncryptHelper.EncryptBytes(Library.ObjectHelper.SerializeObject(dict));
                item.Timeout = 15000;//超时时间
                Library.HttpResult result = new Library.HttpResult();

                result = http.GetHtml(item);//获取页面方法
                try
                {
                    byte[] bs = result.ResultByte;
                    if (bs != null)
                    {
                        resultDic = (Dictionary<int, Object>)Library.ObjectHelper.DeserializeObject(Library.DEncryptHelper.DecryptBytes(bs));
                    }
                }
                catch
                {
                    resultDic.Add(0, result.Html);
                }
            }
            catch (Exception ex)
            {
                Log.LogSave(RankingLog, "连接上传接口时出错【" + ex.Message + "】");
            }
            return resultDic;
        }
    }
}
