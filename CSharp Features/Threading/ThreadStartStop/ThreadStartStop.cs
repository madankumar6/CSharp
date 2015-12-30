using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadStartStop
{
    public class Worker
    {
        private volatile bool shouldStop = false;
        
        // This method will be called when the thread is started.
        public void DoWork()
        {
            while (!shouldStop)
            {
                Console.WriteLine("worker thread: working...");
            }
        }

        public void RequestStop()
        {
            shouldStop = true;
        }
    }
}
