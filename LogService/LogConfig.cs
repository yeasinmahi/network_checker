using System;

namespace LogService
{
    public class LogConfig
    {
        public static LogUtility.BuildMode BuildMode = LogUtility.BuildMode.Debug;
        //public static string DefaultPath => Environment.CurrentDirectory+"\\"+ DateTime.Now.ToString("dd-MMM-yyyy")+ "\\Log.txt";
        public static string DefaultPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"\\NetworkStatusChecker\\" + DateTime.Now.ToString("dd-MMM-yyyy") + "\\Log.txt";
        public static string EventLogPath = "C:/NetworkChecker/Event.txt";
        public static string ErrorLogPath = "C:/NetworkChecker/Errors.txt";
        private static object _locker = new object();

        public static object GetLocker()
        {
            if (_locker==null)
            {
                _locker = new object();
            }
            return _locker;
        }
    }
}
