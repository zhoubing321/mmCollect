using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
   public class tb_task_log
    {
        private int _task_log_id = 0;

        public int task_log_id
        {
            get { return _task_log_id; }
            set { _task_log_id = value; }
        }
        private string _task_log_key = "";

        public string task_log_key
        {
            get { return _task_log_key; }
            set { _task_log_key = value; }
        }
        private int _task_log_task = 0;

        public int task_log_task
        {
            get { return _task_log_task; }
            set { _task_log_task = value; }
        }
        private int _task_log_user = 0;

        public int task_log_user
        {
            get { return _task_log_user; }
            set { _task_log_user = value; }
        }

        private int _task_log_task_user = 0;

        public int task_log_task_user
        {
            get { return _task_log_task_user; }
            set { _task_log_task_user = value; }
        }
        private string _task_log_ip = "";

        public string task_log_ip
        {
            get { return _task_log_ip; }
            set { _task_log_ip = value; }
        }
        private int _task_log_status = 0;

        public int task_log_status
        {
            get { return _task_log_status; }
            set { _task_log_status = value; }
        }
        private int _task_log_score = 0;

        public int task_log_score
        {
            get { return _task_log_score; }
            set { _task_log_score = value; }
        }
        private int _task_log_sort = 0;

        public int task_log_sort
        {
            get { return _task_log_sort; }
            set { _task_log_sort = value; }
        }
        private DateTime _task_log_createtime = DateTime.Now;

        public DateTime task_log_createtime
        {
            get { return _task_log_createtime; }
            set { _task_log_createtime = value; }
        }
        private DateTime _task_log_endtime = DateTime.Now;

        public DateTime task_log_endtime
        {
            get { return _task_log_endtime; }
            set { _task_log_endtime = value; }
        }
    }
}
