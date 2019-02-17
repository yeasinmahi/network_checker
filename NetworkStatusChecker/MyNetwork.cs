using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkStatusChecker
{

    public class MyNetwork
    {
        private static readonly Ping P = new Ping();
        private static PingReply _r;
        
        private static readonly WebClient WebClient = new WebClient() { Credentials = new NetworkCredential("erp", "erp123") };
        private static Stopwatch _sw;
        private static FileInfo _fileInfo;
        private static double _speed;
        public enum Type
        {
            Download,
            Upload
        }
        private static double GetSpeed(Type type)
        {
            _sw = Stopwatch.StartNew();
            _speed = 0;
            try
            {

                if (type.Equals(Type.Download))
                {
                    WebClient.DownloadFile(Messages.DownloadUrl, Messages.Tempfile);
                    _sw.Stop();
                    _fileInfo = new FileInfo(Messages.Tempfile);
                    _speed =  (_fileInfo.Length / (_sw.Elapsed.Milliseconds * 1.024*1024));
                }
                else
                {
                    WebClient.UploadData(Messages.UploadUrl, Properties.Resources.a);
                    _sw.Stop();
                    _speed =  (Properties.Resources.a.Length / (_sw.Elapsed.Milliseconds * 1.024*1024));
                }
                
            }
            catch
            {
                return 0;
            }
            FileHelper.DeleteFile(type.Equals(Type.Download) ? Messages.Tempfile : Messages.UploadUrl);

            return _speed;
            
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
                _r = P.Send(Messages.PingUrl);
            }
            catch
            {
                return false;
            }

            return _r != null && _r.Status == IPStatus.Success;
        }

        public static double GetUploadSpeed()
        {
            return GetSpeed(Type.Upload);
        }
    }
}
