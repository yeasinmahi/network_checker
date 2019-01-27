using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NetworkStatusChecker
{
    public class MyTimer
    {
        public static Timer GetTimer(int interval, ElapsedEventHandler eventHandler)
        {
            Timer timer = new System.Timers.Timer();
            timer.Interval = 5;
            timer.Elapsed += new ElapsedEventHandler(eventHandler);
            return timer;
        }
    }
}
