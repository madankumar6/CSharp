using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Quantifiers
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Any - Simple \n 2. Any - Grouped \n 3. All - Simple \n 4. All - Grouped");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        AnySimple();
                        break;
                    case 2:
                        AnyGrouped();
                        break;
                    case 3:
                        AllSimple();
                        break;
                    case 4:
                        AllGrouped();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void AnySimple()
        {
            Console.WriteLine("This sample uses Any to determine if any of the words in the array contain the substring 'ei'.");

            string[] words = { "believe", "relief", "receipt", "field" };

            bool iAfterE = words.Any(word => word.IndexOf("ei") > 0);

            //bool iAfterE = words.Any(word => word.Contains("ei"));

            Console.WriteLine("There is a word that contains in the list that contains 'ei': {0}", iAfterE); 
        }

        private static void AnyGrouped()
        {
            Console.WriteLine("This sample uses Any to return a grouped a list of products only for categories that have at least one product that is out of stock.");

            List<Product> products = factory.GetProductList();
            var productGroups = from product in products
                                group product by product.Category into productGroup
                                where productGroup.Any(pg => pg.UnitsInStock <= 0)
                                select new
                                {
                                    Category = productGroup.Key,
                                    Products = productGroup
                                };

            foreach (var item in productGroups)
            {
                Console.WriteLine("Categorqy = {0}, Products : ", item.Category);

                foreach (var productInfo in item.Products)
                {
                    Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
                }
            }
            
        }

        private static void AllSimple()
        {
            Console.WriteLine("This sample uses All to determine whether an array contains only odd numbers.");

            int[] numbers = { 1, 11, 3, 19, 41, 65, 19 };

            bool onlyOdd = numbers.All(number => number % 2 != 0);

            Console.WriteLine("The list contains only odd numbers: {0}", onlyOdd); 
        }

        private static void AllGrouped()
        {
            Console.WriteLine("This sample uses All to return a grouped a list of products only for categories that have all of their products in stock.");

            List<Product> products = factory.GetProductList();

            var productGroups = from product in products
                                group product by product.Category into productGroup
                                where productGroup.All(pg => pg.UnitsInStock <= 0)
                                select new
                                {
                                    Category = productGroup.Key,
                                    Products = productGroup
                                };

            foreach (var item in productGroups)
            {
                Console.WriteLine("Categorqy = {0}, Products : ", item.Category);

                foreach (var productInfo in item.Products)
                {
                    Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
                }
            }
        }
    }
}
