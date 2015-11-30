using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionClassI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Tokens by breaking the string into tokens:");

            Tokens token = new Tokens("This is a well-done program.", new char[] { ' ', '-' });

            foreach (string item in token)
            {
                Console.WriteLine(item);
            }

            Console.Read();
        }
    }
}
