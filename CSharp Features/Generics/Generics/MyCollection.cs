using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{

    //Type parameter T in angle brackets.
    public class MyList<T> : IEnumerable<T>
    {
        protected Node head;
        protected Node current = null;
        
        protected class Node
        {
            public Node next;
            //T as private member datatype.
            private T data;
            //T used in non-generic constructor.
            public Node(T dataNode)
            {
                next = null;
                data = dataNode;
            }

            public Node Next
            {
                get { return next; }
                set { next = value; }
            }

            //T as return type of property.
            public T Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        public MyList()
        {
            head = null;
        }

        //T as method parameter type.
        public void AddHead(T data)
        {
            Node n = new Node(data);
            n.Next = head;
            head = n;
        }

        // Implement GetEnumerator to return IEnumerator<T> to enable foreach iteration of our list. 
        // Note that in C# 2.0 you are not required to implement Current and MoveNext.
        // The compiler will create a class that implements IEnumerator<T>.
        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current.Next != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        // We must implement this method because IEnumerable<T> inherits IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
