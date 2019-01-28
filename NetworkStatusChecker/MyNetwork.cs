using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkStatusChecker
{

    public class MyNetwork
    {
        public enum Type
        {
            Download,
            Upload
        }
        private static double GetSpeed(Type type)
        {
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential("erp", "erp123");
            Stopwatch sw = Stopwatch.StartNew();
            FileInfo fileInfo;
            double speed = 0;
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
            catch(Exception ex)
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

        public static double GetUploadSpeed()
        {
            return GetSpeed(Type.Upload);
        }
    }
}
