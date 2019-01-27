

using System.IO;

namespace NetworkStatusChecker
{
    public static class Messages
    {
        public static readonly string Tempfile = Path.Combine(Path.GetTempPath(),"tempfile.tmp");
        public const string SpeedHead = "Speed Information";
        public const string NetworkHead = "Network Information";
        public const string NetworkMessageUp = "Network Connection is ok";
        public const string NetworkMessageDown = "Network Connection is down";

        public static string GetSpeedMessage(double downloadSpeed)
        {
            return "Download:" + downloadSpeed + " Kbps.";
        }
    }
}
