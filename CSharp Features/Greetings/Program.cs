using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greetings
{
    class Program
    {

        public delegate void Greetings(string name);

        static void Main(string[] args)
        {
        }

        public static void Hello(string name)
        {
            Console.WriteLine("  Hello, {0}!", name);
        }

        public static void Goodbye(string name)
        {
            Console.WriteLine("  Goodbye, {0}!", name);
        }
    }
}
