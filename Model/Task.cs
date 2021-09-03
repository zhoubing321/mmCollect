using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Task
    {
        private string task_word = "";

        public string Task_word1
        {
            get { return task_word; }
            set { task_word = value; }
        }
        private string task_domain = "";

        public string Task_domain1
        {
            get { return task_domain; }
            set { task_domain = value; }
        }

        //public string Task_word { get => task_word; set => task_word = value; }
        //public string Task_domain { get => task_domain; set => task_domain = value; }
    }
}
