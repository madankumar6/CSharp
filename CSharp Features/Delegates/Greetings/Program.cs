using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greetings
{
    class Program
    {
        public delegate void GreetingsDelegate(string name);

        static void Main(string[] args)
        {
            GreetingsDelegate hello, goodbye, helloGoodbye, goodbyeOnly ;

            hello = Hello;
            goodbye = Goodbye;

            helloGoodbye = hello + goodbye;


            Console.WriteLine("Invoking delegate Hello:");
            hello("Madankumar");
            Console.WriteLine("Invoking delegate Goodbye:");
            goodbye("Madankumar");
            Console.WriteLine("Invoking delegate HelloGoodbye:");
            helloGoodbye("Madankumar");

            goodbyeOnly = helloGoodbye - hello;
            Console.WriteLine("Invoking delegate HelloGoodbye:");
            goodbyeOnly("Madankumar");

            Console.Read();
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
