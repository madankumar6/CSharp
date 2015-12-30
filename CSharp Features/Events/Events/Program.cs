using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new list.
            CustomEventsListWithChangedEvent list = new CustomEventsListWithChangedEvent();

            // Create a class that listens to the list's change event.
            EventListener listener = new EventListener(list);

            // Add and remove items from the list.
            list.Add("item 1");
            list.Clear();
            listener.Detach();
            Console.Read();
        }
    }
}
