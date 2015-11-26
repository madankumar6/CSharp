using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Query_Execution
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Deferred Execution \n 2. Immediate Execution \n 3. Query Reuse");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        DeferredExecution();
                        break;
                    case 2:
                        ImmediateExecution();
                        break;
                    case 3:
                        QueryReuse();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void DeferredExecution()
        {
            Console.WriteLine("The following sample shows how query execution is deferred until the query is enumerated at a foreach statement.");
            
            int[] numbers = new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
            int i = 0;
            var q = from n in numbers
                    select ++i;

            foreach (var v in q)
            {
                Console.WriteLine("v = {0}, i = {1}", v, i);
            } 
        }

        private static void ImmediateExecution()
        {
            Console.WriteLine("The following sample shows how queries can be executed immediately with operators such as ToList().");

            int[] numbers = new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int i = 0;
            var q = (from n in numbers
                    select ++i).ToList();

            foreach (var v in q)
            {
                Console.WriteLine("v = {0}, i = {1}", v, i);
            } 
        }

        private static void QueryReuse()
        {
            Console.WriteLine("The following sample shows how, because of deferred execution, queries can be used again after data changes and will then operate on the new data.");

            int[] numbers = new int[] { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var lowNumbers = from number in numbers
                             where number <= 3
                             select number;

            Console.WriteLine("First run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            }

            for (int i = 0; i < 10; i++)
            {
                numbers[i] = -numbers[i];
            }

            Console.WriteLine("Second run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            } 
        }
    }


}
