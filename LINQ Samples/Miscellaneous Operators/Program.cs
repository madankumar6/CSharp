using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Miscellaneous_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. ConcatI \n 2. ConcatII \n 3. EqualAllI \n 4. EqualAllII");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        ConcatI();
                        break;
                    case 2:
                        ConcatII();
                        break;
                    case 3:
                        EqualAllI();
                        break;
                    case 4:
                        EqualAllII();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void ConcatI()
        {
            Console.WriteLine("This sample uses Concat to create one sequence that contains each array's values, one after the other.");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var allNumbers = numbersA.Concat(numbersB);

            Console.WriteLine("All numbers from both arrays:");
            foreach (var n in allNumbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void ConcatII()
        {
            Console.WriteLine("This sample uses Concat to create one sequence that contains the names of all customers and products, including any duplicates.");

            List<Customer> customers = factory.GetCustomerList();
            List<Product> products = factory.GetProductList();

            var customerNames = from customer in customers
                                select customer.CompanyName;

            var productNames = from product in products
                               select product.ProductName;

            var allNames = customerNames.Concat(productNames);

            Console.WriteLine("Customer and product names:");
            foreach (var n in allNames)
            {
                Console.WriteLine(n);
            } 
        }

        private static void EqualAllI()
        {
            Console.WriteLine("This sample uses EqualAll to see if two sequences match on all elements in the same order.");

            var wordsA = new string[] { "cherry", "apple", "blueberry" }; 
            var wordsB = new string[] { "cherry", "apple", "blueberry" };

            bool match = wordsA.SequenceEqual(wordsB);

            Console.WriteLine("The sequences match: {0}", match); 
        }

        private static void EqualAllII()
        {
            Console.WriteLine("This sample uses EqualAll to see if two sequences match on all elements in the same order.");

            var wordsA = new string[] { "cherry", "apple", "blueberry" };
            var wordsB = new string[] { "apple", "blueberry", "cherry" };

            bool match = wordsA.SequenceEqual(wordsB);

            Console.WriteLine("The sequences match: {0}", match);
        }
    }
}
