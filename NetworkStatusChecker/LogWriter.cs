using System;
using LogService;

namespace NetworkStatusChecker
{
    public class LogWriter
    {
        public static void WriteLog(string action, double downloadSpeed, double uploadSpeed )
        {
            string localIpAddress = Network.GetLocalIpAddress();
            dynamic obj = new
            {
                Environment.UserDomainName,
                Environment.UserName,
                Environment.MachineName,
                Environment.OSVersion.Platform,
                localIpAddress,
                action,
                downloadSpeed,
                uploadSpeed
            };
            Log.Instance.CustomLog(obj);
        }
    }
}
