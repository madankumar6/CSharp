using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Element_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. First - Simple \n 2. First - Condition \n 3. FirstOrDefault - Simple \n 4. FirstOrDefault - Condition \n 5. ElementAt");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        FirstSimple();
                        break;
                    case 2:
                        FirstCondition();
                        break;
                    case 3:
                        FirstOrDefaultSimple();
                        break;
                    case 4:
                        FirstOrDefaultCondition();
                        break;
                    case 5:
                        ElementAt();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void FirstSimple()
        {
            Console.WriteLine("This sample uses First to return the first matching element as a Product, instead of as a sequence containing a Product.");

            List<Product> products = factory.GetProductList();

            Product productInfo = (from product in products
                                 where product.ProductID == 12
                                 select product).First();

            Console.WriteLine("ProductID = {0}, ProductName = {1} Price = {2}.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
        }

        private static void FirstCondition()
        {
            Console.WriteLine("This sample uses First to find the first element in the array that starts with 'o'.");

            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 

            var startsWithO = strings.First(stringWord => stringWord.StartsWith("o"));

            Console.WriteLine("A string starting with 'o': {0}", startsWithO); 
        }

        private static void FirstOrDefaultSimple()
        {
            Console.WriteLine("This sample uses FirstOrDefault to try to return the first element of the sequence, unless there are no elements, in which case the default value for that type is returned.");

            int[] numbers = { };

            int firstNumOrDefault = numbers.FirstOrDefault();

            Console.WriteLine(firstNumOrDefault); 
        }

        private static void FirstOrDefaultCondition()
        {
            Console.WriteLine("This sample uses FirstOrDefault to return the first product whose ProductID is 789 as a single Product object, unless there is no match, in which case null is returned.");
            List<Product> products = factory.GetProductList();

            Product product789 = products.FirstOrDefault(product => product.ProductID == 789);

            Console.WriteLine("Product 789 exists: {0}", product789 != null); 
        }

        private static void ElementAt()
        {
            Console.WriteLine("This sample uses ElementAt to retrieve the second number greater than 5 from an array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int fourthLowNum = (from number in numbers
                                where number > 5
                                //orderby number
                                select number).ElementAt(1);

            Console.WriteLine("Second number > 5: {0}", fourthLowNum); 
        }
    }
}
