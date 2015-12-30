using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.TraceMessage("Main Starting");

            if (args.Length == 0 )
            {
                Console.WriteLine("No arguments have been passed"); 
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("Arg[{0}] is [{1}]", i, args[i]); 
                }
            }

            Trace.TraceMessage("Main Ending");

            Console.Read();
        }
    }
}
