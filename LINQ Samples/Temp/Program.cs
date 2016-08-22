using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Temp
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> cities = new[] {"Chennai", "Mumbai", "Coimbatore", "Delhi", "Bangalore"};

            var citiesStartsWithC = cities.Filter(CitiesStartsWithC);

            citiesStartsWithC = cities.Filter(delegate(string item) { return item.StartsWith("M"); });
            citiesStartsWithC = cities.Filter(item => item.StartsWith("B"));

            foreach (string s in citiesStartsWithC)
            {
                Console.WriteLine(s);
            }
        }

        static bool CitiesStartsWithC(string city)
        {
            return city.StartsWith("C");
        }
    }
}
