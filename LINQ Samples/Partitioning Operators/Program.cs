using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Partitioning_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Take - Simple I \n 2. Take - Nested \n 3. Skip - Simple \n 4. Skip - Nested \n 5. TakeWhile - Simple \n 6. TakeWhile - Indexed \n 7. SkipWhile - Simple \n 8. SkipWhile - Indexed");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        TakeSimple();
                        break;
                    case 2:
                        TakeNested();
                        break;
                    case 3:
                        SkipSimple();
                        break;
                    case 4:
                        SkipNested();
                        break;
                    case 5:
                        TakeWhileSimple();
                        break;
                    case 6:
                        TakeWhileIndexed();
                        break;
                    case 7:
                        SkipWhileSimple();
                        break;
                    case 8:
                        SkipWhileIndexed();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void TakeSimple()
        {
            Console.WriteLine("This sample uses Take to get only the first 3 elements of the array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var first3Numbers = numbers.Take(3);

            Console.WriteLine("First 3 numbers:");

            foreach (var n in first3Numbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void TakeNested()
        {
            Console.WriteLine("This sample uses Take to get the first 3 orders from customers in Washington.");

            List<Customer> customers = factory.GetCustomerList();

            var first3WAOrders = from customer in customers
                                 where customer.Region == "WA"
                                 from order in customer.Orders.Take(3)
                                 select new { customer.CustomerID, order.OrderID, order.OrderDate };

            Console.WriteLine("First 3 orders in WA:");

            foreach (var order in first3WAOrders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Total = {2}", order.CustomerID, order.OrderID, order.OrderDate);
            }
        }

        private static void SkipSimple()
        {
            Console.WriteLine("This sample uses Skip to get all but the first 4 elements of the array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var allButFirst4Numbers = numbers.Skip(4);

            Console.WriteLine("All but first 4 numbers:");

            foreach (var n in allButFirst4Numbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void SkipNested()
        {
            Console.WriteLine("This sample uses Take to get all but the first 2 orders from customers in Washington.");

            List<Customer> customers = factory.GetCustomerList();

            var allButFirst2Orders = from customer in customers
                                 where customer.Region == "WA"
                                 from order in customer.Orders.Skip(2)
                                 select new { customer.CustomerID, order.OrderID, order.OrderDate };

            Console.WriteLine("First 3 orders in WA:");

            foreach (var order in allButFirst2Orders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Total = {2}", order.CustomerID, order.OrderID, order.OrderDate);
            }
        }

        private static void TakeWhileSimple()
        {
            Console.WriteLine("This sample uses TakeWhile to return elements starting from the beginning of the array until a number is hit that is not less than 6.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstNumbersLessThan6 = numbers.TakeWhile(number => number < 6);

            Console.WriteLine("First numbers less than 6:");

            foreach (var n in firstNumbersLessThan6)
            {
                Console.WriteLine(n);
            } 
        }

        private static void TakeWhileIndexed()
        {
            Console.WriteLine("This sample uses TakeWhile to return elements starting from the beginning of the array until a number is hit that is less than its position in the array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var firstSmallNumbers = numbers.TakeWhile((number, index) => number >= index);

            Console.WriteLine("First numbers not less than their position:");

            foreach (var n in firstSmallNumbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void SkipWhileSimple()
        {
            Console.WriteLine("This sample uses SkipWhile to get the elements of the array starting from the first element divisible by 3.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var allButFirst3Numbers = numbers.SkipWhile(number => number % 3 != 0);

            Console.WriteLine("All elements starting from first element divisible by 3:");

            foreach (var n in allButFirst3Numbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void SkipWhileIndexed()
        {
            Console.WriteLine("This sample uses SkipWhile to get the elements of the array starting from the first element less than its position.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var laterNumbers = numbers.SkipWhile((number, index) => number >= index);

            Console.WriteLine("All elements starting from first element less than its position:");

            foreach (var n in laterNumbers)
            {
                Console.WriteLine(n);
            } 
        }
    }
}
