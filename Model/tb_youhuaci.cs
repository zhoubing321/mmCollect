using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    [Serializable]
    public class tb_youhuaci
    {
        private int id;//编号
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int channelID;//频道id
        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        private int classID;//栏目编号
        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        private string ip;//查询电脑的内网ip
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private string internetIp;//查询电脑的外网ip
        public string InternetIp
        {
            get { return internetIp; }
            set { internetIp = value; }
        }

        private string clientVer;//查询电脑挂机端的版本号
        public string ClientVer
        {
            get { return clientVer; }
            set { clientVer = value; }
        }

        private string clientLogUser;//查询电脑的登录用户名
        public string ClientLogUser
        {
            get { return clientLogUser; }
            set { clientLogUser = value; }
        }

        private string rankRange;//查询该词的排名范围 排名<8为到位  排名>8为不到位 排名=0为无排名
        public string RankRange
        {
            get { return rankRange; }
            set { rankRange = value; }
        }

        private DateTime createtime = new DateTime();//该词的查询时间
        public DateTime Createtime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        private string title;//关键词
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string domain;//查询结果的域名
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private string url;//查询结果的完整网址
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private int rank;//查询结果的排名
        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        private string content;//该关键词的查询历史日志记录
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string rankType;//该排名的类型--PC还是SJ
        public string RankType
        {
            get { return rankType; }
            set { rankType = value; }
        }

        private string keywordType;//该关键词的类型--重点词、普通词、品牌词、活动词、竞价词
        public string KeywordType
        {
            get { return keywordType; }
            set { keywordType = value; }
        }

        private string titleMatter;//标题物料
        public string TitleMatter
        {
            get { return titleMatter; }
            set { titleMatter = value; }
        }
    }
}
