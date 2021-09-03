using System;
using System.Collections.Generic;
using System.Text; 

namespace Model
{
    [Serializable]
    public class tb_indentInfo
    {
        private string user;//用户名
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string pwd;//密码
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string rzm;//认证码
        public string Rzm
        {
            get { return rzm; }
            set { rzm = value; }
        }

        private int plan;//加密方式
        public int Plan
        {
            get { return plan; }
            set { plan = value; }
        }

        private int timeout;//超时时间
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private int action;//超时功能编号
        public int Action
        {
            get { return action; }
            set { action = value; }
        }
    }
}
