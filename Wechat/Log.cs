using System;
using System.IO;
using System.Web;

namespace Wechat
{
    public class Log
    {
        //在网站根目录下创建日志目录
        public static string path = String.Empty;

        static Log()
        {
            try
            {
                path = HttpRuntime.AppDomainAppPath.ToString() + "app_data/wxlogs";
            }
            catch { }
        }

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            if (Config.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", className, content);
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            if (Config.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", className, content);
            }
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            if (Config.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", className, content);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static void WriteLog(string type, string className, string content)
        {
            if (String.IsNullOrEmpty(path))
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            //创建或打开日志文件，向日志文件末尾追加记录
            using (StreamWriter mySw = File.AppendText(filename))
            {
                //向日志文件写入内容
                mySw.WriteLine(String.Format("{0} {1} {2} {3}", DateTime.Now, type, className, content));

                //关闭日志文件
                mySw.Close();
            }
        }
    }
}
