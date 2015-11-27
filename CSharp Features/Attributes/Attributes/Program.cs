using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    class Program
    {
        static void Main(string[] args)
        {
            // display attributes for Account class
            DisplayAttributes(typeof(OrderAccount));

            // display list of tested members
            foreach (MemberInfo method in typeof(OrderAccount).GetMethods())
            {
                if (IsMemberTested(method))
                {
                    Console.WriteLine("Method is tested. Method Name = {0}", method.Name);
                }
                else
                {
                    Console.WriteLine("Member {0} is NOT tested!", method.Name);
                }
            }

            Console.WriteLine();

            // display attributes for Account class
            DisplayAttributes(typeof(Order));

            // display list of tested members
            foreach (MemberInfo method in typeof(Order).GetMethods())
            {
                if (IsMemberTested(method))
                {
                    Console.WriteLine("Method is tested. Method Name = {0}", method.Name);
                }
                else
                {
                    Console.WriteLine("Member {0} is NOT tested!", method.Name);
                }
            }
            Console.Read();
        }

        private static void DisplayAttributes(MemberInfo member)
        {
            Console.WriteLine("Attributes for : " + member.Name);

            foreach (var attribute in member.GetCustomAttributes(true))
            {
                Console.WriteLine(attribute);
            }
        }

        private static bool IsMemberTested(MemberInfo member)
        {
            foreach (var attribute in member.GetCustomAttributes(true))
            {
                if (attribute is IsTestedAttribute)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
