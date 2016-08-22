using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipsAndTraps
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "";
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Yes its empty");
            }
            name = "   ";
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("No its not empty");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Yes its empty");
            }

            name = String.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Yes its empty");
            }

            //DoNotCallVirtualInContructor();
        }

        private static void DoNotCallVirtualInContructor()
        {
            var baseClass = new BaseClass();
            Console.WriteLine(baseClass.Name);
            var derivedClass = new DerivedClass();
            Console.WriteLine(derivedClass.Name);
        }
    }
}
