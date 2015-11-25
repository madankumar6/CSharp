using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;
using System.Collections;
using System.Data.Linq;
using System.Data;

namespace Custom_Sequence_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Combine");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Combine();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void Combine()
        {
            Console.WriteLine("This sample calculates the dot product of two integer vectors. It uses a user-created sequence operator, Combine, to calculate the dot product, passing it a lambda function to multiply two arrays, element by element, and sum the result.");

            int[] vectorA = { 0, 2, 4, 5, 6 };
            int[] vectorB = { 1, 3, 5, 7, 8 };

            var dotProduct = vectorA.Combine(vectorB, (a, b) => a * b).Sum();

            Console.WriteLine("Dot product: {0}", dotProduct); 
        }
    }

    public static class CustomSequenceOperators 
    {
        public static IEnumerable<int> Combine(this IEnumerable<int> first, IEnumerable<int> second, Func<int, int, int> func)
        {
            using (IEnumerator<int> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(Convert.ToInt32(e1.Current), Convert.ToInt32(e2.Current));
                }
            }
        }
    }

    //public static class CustomSequenceOperators
    //{
    //    public static IEnumerable<T> Combine<T>(this IEnumerable<T> first, IEnumerable<T> second, System.Func<T, T, T> func) where T : Type
    //    {
    //        using (IEnumerator<T> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
    //        {
    //            while (e1.MoveNext() && e2.MoveNext())
    //            {
    //                yield return func(e1.Current, e2.Current);
    //            }
    //        }
    //    }
    //} 

}
