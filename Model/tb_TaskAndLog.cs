using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    [Serializable] 
    public class tb_TaskAndLog
    {
        #region tb_task
        private int _task_id = 0;

        public int task_id
        {
            get { return _task_id; }
            set { _task_id = value; }
        }
        private int _task_user = 0;

        public int task_user
        {
            get { return _task_user; }
            set { _task_user = value; }
        }
        private int _task_type = 0;

        public int task_type
        {
            get { return _task_type; }
            set { _task_type = value; }
        }
        private string _task_word = "";

        public string task_word
        {
            get { return _task_word; }
            set { _task_word = value; }
        }
        private string _task_domain = "";

        public string task_domain
        {
            get { return _task_domain; }
            set { _task_domain = value; }
        }
        private int _task_sort = 0;

        public int task_sort
        {
            get { return _task_sort; }
            set { _task_sort = value; }
        }
        private int _task_sort_new = 0;

        public int task_sort_new
        {
            get { return _task_sort_new; }
            set { _task_sort_new = value; }
        }
        private int _task_search_page_max = 0;

        public int task_search_page_max
        {
            get { return _task_search_page_max; }
            set { _task_search_page_max = value; }
        }
        private int _task_status = 0;

        public int task_status
        {
            get { return _task_status; }
            set { _task_status = value; }
        }
        private string _task_status_str = "";

        public string task_status_str
        {
            get { return _task_status_str; }
            set { _task_status_str = value; }
        }
        private string _task_province = "";

        public string task_province
        {
            get { return _task_province; }
            set { _task_province = value; }
        }
        private int _task_page = 0;

        public int task_page
        {
            get { return _task_page; }
            set { _task_page = value; }
        }
        private string _task_page1_url = "";

        public string task_page1_url
        {
            get { return _task_page1_url; }
            set { _task_page1_url = value; }
        }
        private int _task_page1_time = 0;

        public int task_page1_time
        {
            get { return _task_page1_time; }
            set { _task_page1_time = value; }
        }
        private string _task_page2_url = "";

        public string task_page2_url
        {
            get { return _task_page2_url; }
            set { _task_page2_url = value; }
        }
        private int _task_page2_time = 0;

        public int task_page2_time
        {
            get { return _task_page2_time; }
            set { _task_page2_time = value; }
        }
        private string _task_page3_url = "";

        public string task_page3_url
        {
            get { return _task_page3_url; }
            set { _task_page3_url = value; }
        }
        private int _task_page3_time = 0;

        public int task_page3_time
        {
            get { return _task_page3_time; }
            set { _task_page3_time = value; }
        }
        private string _task_page4_url = "";

        public string task_page4_url
        {
            get { return _task_page4_url; }
            set { _task_page4_url = value; }
        }
        private int _task_page4_time = 0;

        public int task_page4_time
        {
            get { return _task_page4_time; }
            set { _task_page4_time = value; }
        }
        private string _task_page5_url = "";

        public string task_page5_url
        {
            get { return _task_page5_url; }
            set { _task_page5_url = value; }
        }
        private int _task_page5_time = 0;

        public int task_page5_time
        {
            get { return _task_page5_time; }
            set { _task_page5_time = value; }
        }
        private string _task_page6_url = "";

        public string task_page6_url
        {
            get { return _task_page6_url; }
            set { _task_page6_url = value; }
        }
        private int _task_page6_time = 0;

        public int task_page6_time
        {
            get { return _task_page6_time; }
            set { _task_page6_time = value; }
        }
        private DateTime _task_createtime = DateTime.Now;

        public DateTime task_createtime
        {
            get { return _task_createtime; }
            set { _task_createtime = value; }
        }
        private DateTime _task_lastruntime = DateTime.Now;

        public DateTime task_lastruntime
        {
            get { return _task_lastruntime; }
            set { _task_lastruntime = value; }
        }
        private int _task_time_count = 0;

        public int task_time_count
        {
            get { return _task_time_count; }
            set { _task_time_count = value; }
        }
        private int _task_time_count_type = 0;

        public int task_time_count_type
        {
            get { return _task_time_count_type; }
            set { _task_time_count_type = value; }
        }
        private int _task_time_count0 = 0;

        public int task_time_count0
        {
            get { return _task_time_count0; }
            set { _task_time_count0 = value; }
        }
        private int _task_time_count1 = 0;

        public int task_time_count1
        {
            get { return _task_time_count1; }
            set { _task_time_count1 = value; }
        }
        private int _task_time_count2 = 0;

        public int task_time_count2
        {
            get { return _task_time_count2; }
            set { _task_time_count2 = value; }
        }
        private int _task_time_count3 = 0;

        public int task_time_count3
        {
            get { return _task_time_count3; }
            set { _task_time_count3 = value; }
        }
        private int _task_time_count4 = 0;

        public int task_time_count4
        {
            get { return _task_time_count4; }
            set { _task_time_count4 = value; }
        }
        private int _task_time_count5 = 0;

        public int task_time_count5
        {
            get { return _task_time_count5; }
            set { _task_time_count5 = value; }
        }
        private int _task_time_count6 = 0;

        public int task_time_count6
        {
            get { return _task_time_count6; }
            set { _task_time_count6 = value; }
        }
        private int _task_time_count7 = 0;

        public int task_time_count7
        {
            get { return _task_time_count7; }
            set { _task_time_count7 = value; }
        }
        private int _task_time_count8 = 0;

        public int task_time_count8
        {
            get { return _task_time_count8; }
            set { _task_time_count8 = value; }
        }
        private int _task_time_count9 = 0;

        public int task_time_count9
        {
            get { return _task_time_count9; }
            set { _task_time_count9 = value; }
        }
        private int _task_time_count10 = 0;

        public int task_time_count10
        {
            get { return _task_time_count10; }
            set { _task_time_count10 = value; }
        }
        private int _task_time_count11 = 0;

        public int task_time_count11
        {
            get { return _task_time_count11; }
            set { _task_time_count11 = value; }
        }
        private int _task_time_count12 = 0;

        public int task_time_count12
        {
            get { return _task_time_count12; }
            set { _task_time_count12 = value; }
        }
        private int _task_time_count13 = 0;

        public int task_time_count13
        {
            get { return _task_time_count13; }
            set { _task_time_count13 = value; }
        }
        private int _task_time_count14 = 0;

        public int task_time_count14
        {
            get { return _task_time_count14; }
            set { _task_time_count14 = value; }
        }
        private int _task_time_count15 = 0;

        public int task_time_count15
        {
            get { return _task_time_count15; }
            set { _task_time_count15 = value; }
        }
        private int _task_time_count16 = 0;

        public int task_time_count16
        {
            get { return _task_time_count16; }
            set { _task_time_count16 = value; }
        }
        private int _task_time_count17 = 0;

        public int task_time_count17
        {
            get { return _task_time_count17; }
            set { _task_time_count17 = value; }
        }
        private int _task_time_count18 = 0;

        public int task_time_count18
        {
            get { return _task_time_count18; }
            set { _task_time_count18 = value; }
        }
        private int _task_time_count19 = 0;

        public int task_time_count19
        {
            get { return _task_time_count19; }
            set { _task_time_count19 = value; }
        }
        private int _task_time_count20 = 0;

        public int task_time_count20
        {
            get { return _task_time_count20; }
            set { _task_time_count20 = value; }
        }
        private int _task_time_count21 = 0;

        public int task_time_count21
        {
            get { return _task_time_count21; }
            set { _task_time_count21 = value; }
        }
        private int _task_time_count22 = 0;

        public int task_time_count22
        {
            get { return _task_time_count22; }
            set { _task_time_count22 = value; }
        }
        private int _task_time_count23 = 0;

        public int task_time_count23
        {
            get { return _task_time_count23; }
            set { _task_time_count23 = value; }
        }
#endregion

        #region tb_task_log
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
        #endregion


        //任务状态
        private int state = 0;

        public int State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
