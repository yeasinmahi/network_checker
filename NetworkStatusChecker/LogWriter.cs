using System;
using LogService;

namespace NetworkStatusChecker
{
    public class LogWriter
    {
        public static void WriteLog()
        {
            dynamic obj = new
            {
                Environment.UserName,
                Environment.MachineName,
                Environment.OSVersion,

            };
            Log.Instance.CustomLog(obj);
        }
    }
}
