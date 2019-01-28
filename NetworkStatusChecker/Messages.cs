using System.IO;

namespace NetworkStatusChecker
{
    public static class Messages
    {
        public static readonly string Tempfile = Path.Combine(Path.GetTempPath(),"tempfile.tmp");
        public static readonly string DownloadUrl = "ftp://ftp.akij.net/InternetConnectionLog/a.exe";
        public static readonly string UploadUrl = "ftp://ftp.akij.net/InternetConnectionLog/b.exe";
        public const string SpeedHead = "Speed Information";
        public const string NetworkHead = "Network Information";
        public const string NetworkMessageUp = "Network Connection is ok";
        public const string NetworkMessageDown = "Network Connection is down";

        public static string GetSpeedMessage(double downloadSpeed, double uploadSpeed)
        {
            return "Download:" + downloadSpeed.ToString("F2") + " Mbps. Upload:"+uploadSpeed.ToString("F2") +" Mbps";
        }
    }
}
