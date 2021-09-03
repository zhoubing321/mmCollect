using System;
using System.IO;
using System.Threading;

namespace Library
{
    public class Log
    {
        public static string RootDirectory = Directory.GetCurrentDirectory();//文件根目录

        /// <summary>
        /// 其他日志文件
        /// </summary>
        /// <param name="pathNow"></param>
        /// <param name="str"></param>
        public static void LogSave(string pathNow, string Str)
        {
            Thread t = new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    lock ("log")
                    {
                        string AppPath = RootDirectory + "\\运行日志";
                        if (!Directory.Exists(AppPath))
                            Directory.CreateDirectory(AppPath);
                        if (!File.Exists(pathNow))
                            File.WriteAllText(pathNow, "");

                        //删除超过7天的日志文件
                        DirectoryInfo dInfo = new DirectoryInfo(RootDirectory + "\\运行日志");
                        FileInfo[] FInfo = dInfo.GetFiles();
                        for (int i = 0; i < FInfo.Length; i++)
                        {
                            FileInfo fi = new FileInfo(RootDirectory + "\\运行日志\\" + FInfo[i].Name);
                            if (fi.CreationTime < DateTime.Now.AddDays(-3)) { fi.Delete(); }
                        }
                        //FileInfo fi = new FileInfo(pathNow);
                        //if (fi.Length >= 3145728)
                        //{
                        //    pathNow = AppPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".txt";
                        //    File.AppendAllText(pathNow, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + Str.Replace("\r\n", "") + "\r\n");
                        //}
                        //else
                        //{
                        File.AppendAllText(pathNow, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + Str.Replace("\r\n", "") + "\r\n");
                        //}
                    }
                }
                catch { }
            }));
            t.IsBackground = true;
            t.Start();
        }
        /// <summary>
        /// 清空大小和时间不符合的日志
        /// </summary>
        public static void ClearLog()
        {
            Thread t = new Thread(new ThreadStart(delegate ()
            {
                while (true)
                {
                    try
                    {
                        if (Directory.Exists(RootDirectory + "\\运行日志"))
                        {
                            //删除超过7天的日志文件
                            DirectoryInfo dInfo = new DirectoryInfo(RootDirectory + "\\运行日志");
                            FileInfo[] FInfo = dInfo.GetFiles();
                            for (int i = 0; i < FInfo.Length; i++)
                            {
                                FileInfo fi = new FileInfo(RootDirectory + "\\运行日志\\" + FInfo[i].Name);
                                if (fi.CreationTime < DateTime.Now.AddDays(-3)) { fi.Delete(); }
                            }
                        }
                    }
                    catch
                    {

                    }
                    Thread.Sleep(86400000);
                }
            }));
            t.Start();
        }

    }
}
