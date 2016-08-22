using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> input, Func<T, bool> predicate)  //FilterDelegate<T> predicate)
        {
            foreach (var item in input)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        } 
    }

    public delegate bool FilterDelegate<T>(T input);
}
