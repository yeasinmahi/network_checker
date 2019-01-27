using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkStatusChecker
{
    public class MyNetwork
    {
        public static double GetDownloadSpeed()
        {
            WebClient webClient = new WebClient();
            Stopwatch sw = Stopwatch.StartNew();
            FileInfo fileInfo;
            try
            {
                webClient.DownloadFile("http://dl.google.com/googletalk/googletalk-setup.exe", Messages.Tempfile);
                sw.Stop();

                fileInfo = new FileInfo(Messages.Tempfile);
            }
            catch
            {
                return 0;
            }
            
            long speed = 0;
            try
            {
                speed = fileInfo.Length / (sw.Elapsed.Seconds * 1024);
            }
            catch
            {
                // ignored
            }
            FileHelper.DeleteFile(Messages.Tempfile);
            return speed;
        }
        public static bool GetNetworkStatus()
        {
            //return NetworkInterface.GetIsNetworkAvailable();
            Ping p = new Ping();
            PingReply r;
            string s;
            s = "www.google.com";

            try
            {
                r = p.Send(s);
            }
            catch
            {
                return false;
            }

            return r.Status == IPStatus.Success;
        }
    }
}
