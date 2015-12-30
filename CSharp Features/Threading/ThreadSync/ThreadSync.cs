using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSync
{
    // The thread synchronization events are encapsulated in this class to allow them to easily be passed to the Consumer and Producer classes. 
    public class SyncEvents
    {
        private WaitHandle[] eventArray;
        private EventWaitHandle newItemEvent;
        private EventWaitHandle exitThreadEvent;

        // Public properties allow safe access to the events.
        public WaitHandle[] EventArray
        {
            get { return eventArray; }
        }

        public EventWaitHandle NewItemEvent
        {
            get { return newItemEvent; }
        }

        public EventWaitHandle ExitThreadEvent
        {
            get { return exitThreadEvent; }
        }

        public SyncEvents()
        {
            // AutoResetEvent is used for the "new item" event because
            // we want this event to reset automatically each time the
            // consumer thread responds to this event.
            newItemEvent = new AutoResetEvent(false);

            // ManualResetEvent is used for the "exit" event because
            // we want multiple threads to respond when this event is
            // signaled. If we used AutoResetEvent instead, the event
            // object would revert to a non-signaled state with after 
            // a single thread responded, and the other thread would 
            // fail to terminate.
            exitThreadEvent = new ManualResetEvent(false);

            // The two events are placed in a WaitHandle array as well so
            // that the consumer thread can block on both events using
            // the WaitAny method.
            eventArray = new WaitHandle[2];
            eventArray[0] = newItemEvent;
            eventArray[1] = exitThreadEvent;
        }
    }

    // The Producer class asynchronously (using a worker thread) adds items to the queue until there are 20 items.
    public class Producer
    {
        private Queue<int> queue;
        private SyncEvents syncEvents;

        public Producer(Queue<int> queue, SyncEvents syncEvents)
        {
            this.queue = queue;
            this.syncEvents = syncEvents;
        }

        public void ThreadRun()
        {
            int count = 0;
            Random random = new Random();

            while (!syncEvents.ExitThreadEvent.WaitOne(0, false))
            {
                lock (((ICollection)queue).SyncRoot)
                {
                    while (queue.Count < 20)
                    {
                        queue.Enqueue(random.Next(0, 100));
                        syncEvents.NewItemEvent.Set();
                        count++;
                    }
                }
            }

            Console.WriteLine("Producer thread: produced {0} items", count);
        }
    }

    // The Consumer class uses its own worker thread to consume items in the queue. The Producer class notifies the Consumer class of new items with the NewItemEvent.
    public class Consumer
    {
        private Queue<int> queue;
        private SyncEvents syncEvents;

        public Consumer(Queue<int> queue, SyncEvents syncEvents)
        {
            this.queue = queue;
            this.syncEvents = syncEvents;
        }

        public void ThreadRun()
        {
            int count = 0;
            
            while (WaitHandle.WaitAny(syncEvents.EventArray) != 1)
            {
                lock (((ICollection)queue).SyncRoot)
                {
                    int item = queue.Dequeue();
                }
                count++;
            }

            Console.WriteLine("Consumer Thread: consumed {0} items", count);
        }
    }
}
