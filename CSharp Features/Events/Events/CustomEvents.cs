using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    // A delegate type for hooking up change notifications.
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    // A class that works just like ArrayList, but sends event notifications whenever the list changes.
    public class CustomEventsListWithChangedEvent : ArrayList
    {
        // An event that clients can use to be notified whenever the elements of the list change.
        public event ChangedEventHandler Changed;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        // Override some of the methods that can change the list;
        // invoke event after each
        public override int Add(object value)
        {
            int i = base.Add(value);
            OnChanged(EventArgs.Empty);
            return i;
        }

        public override void Clear()
        {
            base.Clear();
            OnChanged(EventArgs.Empty);
        }

        public override object this[int index]
        {
            set
            {
                base[index] = value; 
                OnChanged(EventArgs.Empty);
            }
        }
    }

    public class EventListener
    {
        private CustomEventsListWithChangedEvent List;

        public EventListener(CustomEventsListWithChangedEvent list)
        {
            List = list;
            // Add "ListChanged" to the Changed event on "List".
            List.Changed += new ChangedEventHandler(ListChanged);
        }

        private void ListChanged(object sender, EventArgs e)
        {
            Console.WriteLine("This is called when the event fires.");
        }

        public void Detach()
        {
            // Detach the event and delete the list
            List.Changed -= new ChangedEventHandler(ListChanged);
            List = null;
        }
    }
}
