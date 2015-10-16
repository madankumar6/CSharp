using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Restriction_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Where Simple I \n 2. Where Simple II \n 3. Where Simple II \n 4. Where DrillDown \n 5. Where Indexed ");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        WhereSimple1();
                        break;
                    case 2:
                        WhereSimple2();
                        break;
                    case 3:
                        WhereSimple3();
                        break;
                    case 4:
                        WhereDrillDown();
                        break;
                    case 5:
                        WhereIndexed();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }
                
            } while (choice != 0);
            
        }

        public static void WhereSimple1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numbersLessThanFive = from n in numbers
                                     where n <= 5
                                     select n;

            Console.WriteLine("Numbers less than 5 are");

            foreach (int item in numbersLessThanFive)
            {
                Console.WriteLine(item);
            }
        }

        public static void WhereSimple2()
        {
            var productList = factory.GetProductList();

            var soldOutProducts = from product in productList
                                  where product.UnitsInStock == 0
                                  select product;

            Console.WriteLine("Sold out products are");

            foreach (Product item in soldOutProducts)
            {
                Console.WriteLine(item.ProductName);
            }
        }

        public static void WhereSimple3()
        {
            var productList = factory.GetProductList();

            var expensiveInStockProducts = from product in productList
                                  where product.UnitsInStock > 0 && product.UnitPrice >= 3.00M
                                  select product;

            Console.WriteLine("Products in stock and costs more than 3000 are");

            foreach (Product item in expensiveInStockProducts)
            {
                Console.WriteLine(item.ProductName);
            }
        }

        public static void WhereDrillDown()
        {
            List<Customer> customers = factory.GetCustomerList();

            var waCustomers = from customer in customers
                              where customer.Region == "WA"
                              select customer;

            Console.WriteLine("Customers from Washington and their orders:");

            foreach (Customer customer in waCustomers)
            {
                Console.WriteLine("Customer {0}: {1}", customer.CustomerID, customer.CompanyName);

                foreach (Order order in customer.Orders)
                {
                    Console.WriteLine("  Order {0}: {1}", order.OrderID, order.OrderDate); 
                }
            }
        }

        public static void WhereIndexed()
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var shortDigits = digits.Where((digit, index) => digit.Length < index);

            Console.WriteLine("Short digits:");

            foreach (string item in shortDigits)
            {
                Console.WriteLine("The word {0} is shorter than its value.", item);
            }
        }
    }
}
