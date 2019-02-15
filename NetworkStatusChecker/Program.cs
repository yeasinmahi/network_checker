using System;
using System.Threading;
using System.Windows.Forms;

namespace NetworkStatusChecker
{
    static class Program
    {
        private const string AppGuid = "dd76e002-4d0e-4809-afe9-ecfaad80e9f9";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            using (Mutex mutex = new Mutex(false, "Global\\" + AppGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(@"Instance already running");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
