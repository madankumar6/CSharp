using System;
using System.CodeDom;
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

        private static void SelectSimple1()
        {
            Console.WriteLine("This sample uses select to produce a sequence of ints one higher than those in an existing array of ints.");
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var numPlusOne = from number in numbers
                select number + 1;

            Console.WriteLine("Numbers + 1:"); 
            foreach (int number in numPlusOne)
            {
                Console.WriteLine(number);
            }
        }

        private static void SelectSimple2()
        {
            Console.WriteLine("This sample uses select to return a sequence of just the names of a list of products.");
            List<Product> products = factory.GetProductList();

            var productNames = from product in products
                select product.ProductName;

            Console.WriteLine("Product Names:");
            foreach (var productName in productNames)
            {
                Console.WriteLine(productName);
            } 
        }

        private static void SelectTransformation()
        {
            Console.WriteLine("This sample uses select to produce a sequence of strings representing the text version of a sequence of ints.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var numberInString = from number in numbers
                select strings[number];

            Console.WriteLine("Number strings:");
            foreach (var s in numberInString)
            {
                Console.WriteLine(s);
            } 
        }

        private static void SelectAnonymousType1()
        {
            Console.WriteLine("This sample uses select to produce a sequence of the uppercase and lowercase versions of each word in the original array.");

            string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };

            var upperLowerCase = from word in words
                select new
                {
                    UpperCase = word.ToUpper(),
                    LowerCase = word.ToLower()
                };

            foreach (var ul in upperLowerCase)
            {
                Console.WriteLine("Uppercase: {0}, Lowercase: {1}", ul.UpperCase, ul.LowerCase);
            } 
        }

        private static void SelectAnonymousType2()
        {
            Console.WriteLine("This sample uses select to produce a sequence containing text representations of digits and whether their length is even or odd.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var digitOddEvens = from number in numbers
                select new
                {
                    Number = number,
                    IsEven = number%2 == 0
                };

            foreach (var d in digitOddEvens)
            {
                Console.WriteLine("The digit {0} is {1}.", d.Number, d.IsEven ? "even" : "odd"); 
            } 
        }

        private static void SelectAnonymousType3()
        {
            Console.WriteLine("This sample uses select to produce a sequence containing some properties of Products, including UnitPrice which is renamed to Price in the resulting type.");
            List<Product> products = factory.GetProductList();

            var productInfos = from product in products
                select new
                {
                    product.ProductName,
                    product.Category,
                    Price = product.UnitPrice
                };

            Console.WriteLine("Product Info:");
            foreach (var productInfo in productInfos)
            {
                Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductName, productInfo.Category, productInfo.Price);
            } 
        }

        private static void SelectIndexed()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numsInPlace = numbers.Select((number, index) => new { Number = number, InPlace = number == index });

            Console.WriteLine("Number: In-place?");
            foreach (var n in numsInPlace)
            {
                Console.WriteLine("{0}: {1}", n.Number, n.InPlace);
            }
        }

        private static void SelectFiltered()
        {
            Console.WriteLine("This sample combines select and where to make a simple query that returns the text form of each digit less than 5.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var lowNums = numbers.Where(number => number < 5).Select(number => digits[number]);

            Console.WriteLine("Numbers < 5:");
            foreach (var num in lowNums)
            {
                Console.WriteLine(num);
            } 
        }

        private static void SelectManyCompoundFrom1()
        {
            Console.WriteLine("This sample uses a compound from clause to make a query that returns all pairs of numbers from both arrays such that the number from numbersA is less than the number from numbersB.");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var pairs = from a in numbersA
                from b in numbersB
                where a < b
                select new {a, b};

            Console.WriteLine("Pairs where a < b:");
            foreach (var pair in pairs)
            {
                Console.WriteLine("{0} is less than {1}", pair.a, pair.b);
            } 
        }

        private static void SelectManyCompoundFrom2()
        {
            Console.WriteLine("This sample uses a compound from clause to select all orders where the order total is less than 500.00.");

            List<Customer> customers = factory.GetCustomerList();

            var orders = from customer in customers
                from order in customer.Orders
                where order.Total < 500.00M
                select new {customer.CustomerID, order.OrderID, order.Total};

            foreach (var order in orders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Total = {2}", order.CustomerID, order.OrderID, order.Total);
            }
        }

        private static void SelectManyCompoundFrom3()
        {
            Console.WriteLine("This sample uses a compound from clause to select all orders where the order was made in 1998 or later.");

            List<Customer> customers = factory.GetCustomerList(); 

            var orders = from customer in customers
                         from order in customer.Orders
                         where order.OrderDate >= new DateTime(1998, 1, 1)
                         select new { customer.CustomerID, order.OrderID, order.OrderDate };

            foreach (var order in orders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Order Date = {2}", order.CustomerID, order.OrderID, order.OrderDate);
            }
        }

        private static void SelectManyFromAssignment()
        {
            Console.WriteLine("This sample uses a compound from clause to select all orders where the order total is greater than 2000.00 and uses from assignment to avoid requesting the total twice.");

            List<Customer> customers = factory.GetCustomerList();
            var orders = from customer in customers
                         from order in customer.Orders
                         where order.Total > 2000.00M
                         select new { customer.CustomerID, order.OrderID, order.Total };

            foreach (var order in orders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Total = {2}", order.CustomerID, order.OrderID, order.Total);
            }
        }

        private static void SelectManyMultipleFrom()
        {
            Console.WriteLine("This sample uses multiple from clauses so that filtering on customers can be done before selecting their orders. This makes the query more efficient by not selecting and then discarding orders for customers outside of Washington.");

            List<Customer> customers = factory.GetCustomerList();

            DateTime cutoffDate = new DateTime(1997, 1, 1);

            var orders = from customer in customers
                         where customer.Region == "WA"
                         from order in customer.Orders
                         where order.OrderDate >= cutoffDate
                         select new { customer.CustomerID, order.OrderID, order.Total };

            foreach (var order in orders)
            {
                Console.WriteLine("Customer ID = {0}, Order Id = {1}, Total = {2}", order.CustomerID, order.OrderID, order.Total);
            }
        }

        private static void SelectManyIndexed()
        {
            Console.WriteLine("This sample uses an indexed SelectMany clause to select all orders, while referring to customers by the order in which they are returned from the query.");

            List<Customer> customers = factory.GetCustomerList();

            var customerOrders =
                customers.SelectMany(
                    (customer, index) =>
                        customer.Orders.Select(
                            order => String.Format("Customer #{0} has an order with Order Id {1}", index+1, order.OrderID)));

            foreach (var order in customerOrders)
            {
                Console.WriteLine(order);
            }
        }


    }
}
