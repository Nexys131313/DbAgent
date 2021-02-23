using NLog;
using System;

namespace DBAgent
{
    public class ServiceLog
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void WriteLog(string messText, string messType = "", string logName = "")
        {
            if (logName.Trim() != "")
            {
                try
                {
                    logger = LogManager.GetLogger(logName);

                    if (logger == null) logger = LogManager.GetCurrentClassLogger();

                }
                catch (Exception e)
                {
                }
            }
            else
            {
                logger = LogManager.GetCurrentClassLogger();
            }

            string threadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();

            if (messText.Length > 1024) messText.Substring(0, 1024);

            string _ms = "[" + threadId + "]" + " " + "[" + DateTime.Now.ToString() + "]  " + messText.Replace("'", "`");

            if ((messType.Trim().ToUpper() == "ERROR"))
            {
                try
                {
                    switch (messType.ToUpper().Trim())
                    {
                        case "ERROR": logger.Error(_ms); break;
                        default: logger.Info(_ms); break;
                    }
                }
                catch (Exception errWriteLog)
                {
                }
            }
        }

    }
}
