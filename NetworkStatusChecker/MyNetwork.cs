using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkStatusChecker
{

    public class MyNetwork
    {
        private static Ping p = new Ping();
        private static PingReply r;
        private static string url = "www.google.com";
        private static WebClient webClient = new WebClient() { Credentials = new NetworkCredential("erp", "erp123") };
        private static Stopwatch sw;
        private static FileInfo fileInfo;
        private static double speed = 0;
        public enum Type
        {
            Download,
            Upload
        }
        private static double GetSpeed(Type type)
        {
            sw = Stopwatch.StartNew();
            speed = 0;
            try
            {

                if (type.Equals(Type.Download))
                {
                    webClient.DownloadFile(Messages.DownloadUrl, Messages.Tempfile);
                    sw.Stop();
                    fileInfo = new FileInfo(Messages.Tempfile);
                    speed =  (fileInfo.Length / (sw.Elapsed.Milliseconds * 1.024*1024));
                }
                else
                {
                    webClient.UploadData(Messages.UploadUrl, Properties.Resources.a);
                    sw.Stop();
                    speed =  (Properties.Resources.a.Length / (sw.Elapsed.Milliseconds * 1.024*1024));
                }
                
                
            }
            catch
            {
                return 0;
            }
            if (type.Equals(Type.Download))
            {
                FileHelper.DeleteFile(Messages.Tempfile);
            }
            else
            {
                FileHelper.DeleteFile(Messages.UploadUrl);
            }
            
            return speed;
            
        }
        public static double GetDownloadSpeed()
        {
            return GetSpeed(Type.Download);
        }
        public static bool GetNetworkStatus()
        {
            //return NetworkInterface.GetIsNetworkAvailable();
            try
            {
                r = p.Send(url);
            }
            catch
            {
                return false;
            }

            return r.Status == IPStatus.Success;
        }

        public static double GetUploadSpeed()
        {
            return GetSpeed(Type.Upload);
        }
    }
}
