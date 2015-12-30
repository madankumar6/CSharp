using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSync
{
    class Program
    {
        private static void ShowQueueContents(Queue<int> queue)
        {
            // Enumerating a collection is inherently not thread-safe, so it is imperative that the collection be locked throughout
            // the enumeration to prevent the consumer and producer threads from modifying the contents. (This method is called by the primary thread only.)
            lock (((ICollection)queue).SyncRoot)
            {
                foreach (int i in queue)
                {
                    Console.Write("{0} ", i);
                }
            }
            Console.WriteLine();
        }


        static void Main(string[] args)
        {
            // Configure struct containing event information required for thread synchronization.
            SyncEvents syncEvents = new SyncEvents();

            // Generic Queue collection is used to store items to be produced and consumed. In this case 'int' is used.
            Queue<int> queue = new Queue<int>();

            // Create objects, one to produce items, and one to consume. 
            // The queue and the thread synchronization events are passed to both objects.
            Console.WriteLine("Configuring worker threads...");
            Producer producer = new Producer(queue, syncEvents);
            Consumer consumer = new Consumer(queue, syncEvents);

            // Create the thread objects for producer and consumer objects. 
            // This step does not create or launch the actual threads.
            Thread producerThread = new Thread(producer.ThreadRun);
            Thread consumerThread = new Thread(consumer.ThreadRun);

            // Create and launch both threads.     
            Console.WriteLine("Launching producer and consumer threads...");
            producerThread.Start();
            consumerThread.Start();

            // Let producer and consumer threads run for 10 seconds.
            // Use the primary thread (the thread executing this method) to display the queue contents every 2.5 seconds.

            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(2500);
                ShowQueueContents(queue);
            }

            // Signal both consumer and producer thread to terminate.
            // Both threads will respond because ExitThreadEvent is a manual-reset event--so it stays 'set' unless explicitly reset.
            Console.WriteLine("Signaling threads to terminate...");
            syncEvents.ExitThreadEvent.Set();

            // Use Join to block primary thread, first until the producer thread terminates, then until the consumer thread terminates.
            Console.WriteLine("main thread waiting for threads to finish...");
            producerThread.Join();
            consumerThread.Join();

            Console.Read();
        }
    }
}
