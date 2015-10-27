using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Projection_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {

            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Select - Simple I \n 2. Select - Simple II \n 3. Select Transformation \n 4. Select - Anonymous Type I \n 5. Select - Anonymous Type II \n 6. Select - Anonymous Type III \n 7. Select - Indexed \n 8. Select - Filtered \n 9. SelectMany - Compound From I \n 10. SelectMany - Compound From II \n 11. SelectMany - Compound From III \n 12. SelectMany - From Assignment \n 13. SelectMany - Multiple From \n 14. SelectMany - Indexed");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        SelectSimple1();
                        break;
                    case 2:
                        SelectSimple2();
                        break;
                    case 3:
                        SelectTransformation();
                        break;
                    case 4:
                        SelectAnonymousType1();
                        break;
                    case 5:
                        SelectAnonymousType2();
                        break;
                    case 6:
                        SelectAnonymousType3();
                        break;
                    case 7:
                        SelectIndexed();
                        break;
                    case 8:
                        SelectFiltered();
                        break;
                    case 9:
                        SelectManyCompoundFrom1();
                        break;
                    case 10:
                        SelectManyCompoundFrom2();
                        break;
                    case 11:
                        SelectManyCompoundFrom3();
                        break;
                    case 12:
                        SelectManyFromAssignment();
                        break;
                    case 13:
                        SelectManyMultipleFrom();
                        break;
                    case 14:
                        SelectManyIndexed();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);

        }

        private static void SelectManyIndexed()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numPlusOne = from n in numbers
                select n + 1;

            Console.WriteLine("Numbers + 1:");
            foreach (int i in numPlusOne)
            {
                Console.WriteLine(i);
            }
        }

        private static void SelectManyMultipleFrom()
        {
            throw new NotImplementedException();
        }

        private static void SelectManyFromAssignment()
        {
            throw new NotImplementedException();
        }

        private static void SelectManyCompoundFrom3()
        {
            throw new NotImplementedException();
        }

        private static void SelectManyCompoundFrom2()
        {
            throw new NotImplementedException();
        }

        private static void SelectManyCompoundFrom1()
        {
            throw new NotImplementedException();
        }

        private static void SelectFiltered()
        {
            throw new NotImplementedException();
        }

        private static void SelectIndexed()
        {
            throw new NotImplementedException();
        }

        private static void SelectAnonymousType3()
        {
            throw new NotImplementedException();
        }

        private static void SelectAnonymousType2()
        {
            throw new NotImplementedException();
        }

        private static void SelectAnonymousType1()
        {
            throw new NotImplementedException();
        }

        private static void SelectTransformation()
        {
            throw new NotImplementedException();
        }

        private static void SelectSimple2()
        {
            throw new NotImplementedException();
        }

        private static void SelectSimple1()
        {
            throw new NotImplementedException();
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
