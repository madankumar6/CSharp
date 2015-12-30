using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{

    // The Fibonacci class provides an interface for using an auxiliary thread to perform the lengthy Fibonacci(N) calculation.
    // N is provided to the Fibonacci constructor, along with an event that the object signals when the operation is complete.
    // The result can then be retrieved with the FibOfN property.

    public class Fibonacci
    {
        private int n, fibOfN;

        public int N
        {
            get { return n; }
        }

        public int FibOfN
        {
            get { return fibOfN; }
        }

        private ManualResetEvent doneEvent;

        public Fibonacci(int n, ManualResetEvent doneEvent)
        {
            this.n = n;
            this.doneEvent = doneEvent;
        }

        // Recursive method that calculates the Nth Fibonacci number.
        public int Calculate(int n)
        {
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return Calculate(n - 1) + Calculate(n-2);
            }
        }

        // Wrapper method for use with thread pool.
        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int) threadContext;
            Console.WriteLine("thread {0} started...", threadIndex);
            fibOfN = Calculate(n);
            Console.WriteLine("thread {0} result calculated...", threadIndex);
            doneEvent.Set();

        }
    }
}
