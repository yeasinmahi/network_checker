using System.IO;

namespace NetworkStatusChecker
{
    public  class FileHelper
    {
        public static bool DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return true;
        }
    }
}
