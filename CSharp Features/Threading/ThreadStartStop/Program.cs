using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStartStop
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the thread object. This does not start the thread.
            Worker workerObj = new Worker();
            Thread workerThread = new Thread(workerObj.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Starting worker thread...");

            // Loop until worker thread activates.
            while (!workerThread.IsAlive);

            // Put the main thread to sleep for 1000 millisecond to allow the worker thread to do some work:
            Thread.Sleep(1000);

            // Request that the worker thread stop itself:
            workerObj.RequestStop();

            // Use the Join method to block the current thread until the object's thread terminates.
            workerThread.Join();
            Console.WriteLine("main thread: Worker thread has terminated.");
            Console.Read();
        }
    }
}
