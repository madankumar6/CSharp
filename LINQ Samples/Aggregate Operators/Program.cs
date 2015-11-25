using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Aggregate_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Count - Simple \n 2. Count - Conditional \n 3. Count - Nested \n 4. Count - Grouped \n 5. Sum - Simple \n 6. Sum - Projection \n 7. Sum - Grouped \n 8. Min - Simple \n 9. Min - Projection \n 10. Min - Grouped \n 11. Min - Elements \n 12. Max - Simple \n 13. Max - Projection \n 14. Max - Grouped \n 15. Max - Elements \n 16. Average - Simple\n 17. Average - Projection \n 18. Average - Grouped \n 19. Aggregate - Simple \n 20. Aggregate - Seed");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        CountSimple();
                        break;
                    case 2:
                        CountConditional();
                        break;
                    case 3:
                        CountNested();
                        break;
                    case 4:
                        CountGrouped();
                        break;
                    case 5:
                        SumSimple();
                        break;
                    case 6:
                        SumProjection();
                        break;
                    case 7:
                        SumGrouped();
                        break;
                    case 8:
                        MinSimple();
                        break;
                    case 9:
                        MinProjection();
                        break;
                    case 10:
                        MinGrouped();
                        break;
                    case 11:
                        MinElements();
                        break;
                    case 12:
                        MaxSimple();
                        break;
                    case 13:
                        MaxProjection();
                        break;
                    case 14:
                        MaxGrouped();
                        break;
                    case 15:
                        MaxElements();
                        break;
                    case 16:
                        AverageSimple();
                        break;
                    case 17:
                        AverageProjection();
                        break;
                    case 18:
                        AverageGrouped();
                        break;
                    case 19:
                        AggregateSimple();
                        break;
                    case 20:
                        AggregateSeed();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void CountSimple()
        {
            Console.WriteLine("This sample uses Count to get the number of unique factors of 300.");

            int[] factorsOf300 = { 2, 2, 3, 5, 5 };

            var uniqueFactors = factorsOf300.Distinct().Count();

            Console.WriteLine("There are {0} unique factors of 300.", uniqueFactors); 
        }

        private static void CountConditional()
        {
            Console.WriteLine("This sample uses Count to get the number of odd ints in the array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int oddNumbers = numbers.Where(number => number % 2 != 1).Count();

            Console.WriteLine("There are {0} odd numbers in the list.", oddNumbers); 
        }

        private static void CountNested()
        {
            Console.WriteLine("This sample uses Count to return a list of customers and how many orders each has.");

            List<Customer> customers = factory.GetCustomerList();

            var orderCounts = from customer in customers
                              select new
                              {
                                  Customer = customer.CustomerID,
                                  OrderCount = customer.Orders.Count()
                              };

            foreach (var item in orderCounts)
            {
                Console.WriteLine("Customer ID = {0}, Order Count = {1}", item.Customer, item.OrderCount);
            }
        }

        private static void CountGrouped()
        {
            Console.WriteLine("This sample uses Count to return a list of categories and how many products each has.");
            
            List<Product> products = factory.GetProductList();

            var categoryCounts = from product in products
                                 group product by product.Category into productGroup
                                 select new { Category = productGroup.Key, ProductCount = productGroup.Count() };

            foreach (var item in categoryCounts)
            {
                Console.WriteLine("Category  = {0}, Product Count = {1}", item.Category, item.ProductCount);
            }
        }

        private static void SumSimple()
        {
            Console.WriteLine("This sample uses Sum to get the total of the numbers in an array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            double numSum = numbers.Sum();
            
            Console.WriteLine("The sum of the numbers is {0}.", numSum); 
        }

        private static void SumProjection()
        {
            Console.WriteLine("This sample uses Sum to get the total number of characters of all words in the array.");

            string[] words = { "cherry", "apple", "blueberry" };

            double totalChars = words.Sum(word => word.Length);

            Console.WriteLine("There are a total of {0} characters in these words.", totalChars); 
        }
        private static void SumGrouped()
        {
            Console.WriteLine("This sample uses Sum to get the total units in stock for each product category.");

            List<Product> products = factory.GetProductList();

            var categorySum = from product in products
                                 group product by product.Category into productGroup
                                 select new { Category = productGroup.Key, TotalUnitsInStock = productGroup.Sum(product => product.UnitsInStock) };

            foreach (var item in categorySum)
            {
                Console.WriteLine("Category  = {0}, Product Count = {1}", item.Category, item.TotalUnitsInStock);
            }
        }

        private static void MinSimple()
        {
            Console.WriteLine("This sample uses Min to get the lowest number in an array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int minNum = numbers.Min();

            Console.WriteLine("The minimum number is {0}.", minNum); 
        }

        private static void MinProjection()
        {
            Console.WriteLine("This sample uses Min to get the length of the shortest word in an array.");

            string[] words = { "cherry", "apple", "blueberry" };

            int shortestWord = words.Min(word => word.Length);

            Console.WriteLine("The shortest word is {0} characters long.", shortestWord); 
        }

        private static void MinGrouped()
        {
            Console.WriteLine("This sample uses Min to get the cheapest price among each category's products.");

            List<Product> products = factory.GetProductList();

            var categoryMin = from product in products
                                 group product by product.Category into productGroup
                                 select new { Category = productGroup.Key, CheapestPrice = productGroup.Min(product => product.UnitPrice) };

            foreach (var item in categoryMin)
            {
                Console.WriteLine("Category  = {0}, Cheapest Price = {1}", item.Category, item.CheapestPrice);
            }
        }

        private static void MinElements()
        {
            Console.WriteLine("This sample uses Min to get the products with the cheapest price in each category.");

            List<Product> products = factory.GetProductList();

            var categories = from product in products
                                 group product by product.Category into productGroup
                                 let MinPrice = productGroup.Min(product => product.UnitPrice)
                                 select new { Category = productGroup.Key, CheapestPrice = MinPrice, Products = productGroup.Where(product => product.UnitPrice == MinPrice) };

            foreach (var item in categories)
            {
                Console.WriteLine("Category  = {0}, Price = {1}", item.Category, item.CheapestPrice);

                foreach (var productInfo in item.Products)
                {
                    Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
                }
            }
        }

        private static void MaxSimple()
        {
            Console.WriteLine("This sample uses Max to get the highest number in an array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int maxNum = numbers.Max();

            Console.WriteLine("The maximum number is {0}.", maxNum); 
        }

        private static void MaxProjection()
        {
            Console.WriteLine("This sample uses Max to get the length of the longest word in an array.");

            string[] words = { "cherry", "apple", "blueberry" };

            int longestLength = words.Max(word => word.Length);

            Console.WriteLine("The longest word is {0} characters long.", longestLength); 
        }

        private static void MaxGrouped()
        {
            Console.WriteLine("This sample uses Max to get the most expensive price among each category's products.");

            List<Product> products = factory.GetProductList();

            var categoryMax = from product in products
                              group product by product.Category into productGroup
                              select new { Category = productGroup.Key, MostExpensivePrice = productGroup.Max(product => product.UnitPrice) };

            foreach (var item in categoryMax)
            {
                Console.WriteLine("Category  = {0}, Expensive Price = {1}", item.Category, item.MostExpensivePrice);
            }
        }

        private static void MaxElements()
        {
            Console.WriteLine("This sample uses Max to get the products with the most expensive price in each category.");

            List<Product> products = factory.GetProductList();

            var categories = from product in products
                             group product by product.Category into productGroup
                             let MaxPrice = productGroup.Max(product => product.UnitPrice)
                             select new { Category = productGroup.Key, ExpensivePrice = MaxPrice, Products = productGroup.Where(product => product.UnitPrice == MaxPrice) };

            foreach (var item in categories)
            {
                Console.WriteLine("Category  = {0}, Price = {1}", item.Category, item.ExpensivePrice);

                foreach (var productInfo in item.Products)
                {
                    Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
                }
            }
        }

        private static void AverageSimple()
        {
            Console.WriteLine("This sample uses Average to get the average of all numbers in an array.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            double averageNum = numbers.Average();

            Console.WriteLine("The average number is {0}.", averageNum); 
        }

        private static void AverageProjection()
        {
            Console.WriteLine("This sample uses Average to get the average length of the words in the array.");

            string[] words = { "cherry", "apple", "blueberry" };

            double averageLength = words.Average(word => word.Length);

            Console.WriteLine("The average word length is {0} characters.", averageLength); 
        }

        private static void AverageGrouped()
        {
            Console.WriteLine("This sample uses Average to get the average price of each category's products.");

            List<Product> products = factory.GetProductList();

            var categoryAverage = from product in products
                              group product by product.Category into productGroup
                              select new { Category = productGroup.Key, AveragePrice = productGroup.Average(product => product.UnitPrice) };

            foreach (var item in categoryAverage)
            {
                Console.WriteLine("Category  = {0}, Expensive Price = {1}", item.Category, item.AveragePrice);
            }
        }

        private static void AggregateSimple()
        {
            Console.WriteLine("This sample uses Aggregate to create a running product on the array that calculates the total product of all elements.");

            double[] doubles = { 1,2,3,4 };

            double product = doubles.Aggregate((runningProduct, nextFactor) => runningProduct * nextFactor);

            Console.WriteLine("Total product of all numbers: {0}", product); 
        }

        private static void AggregateSeed()
        {
            Console.WriteLine("This sample uses Aggregate to create a running account balance that subtracts each withdrawal from the initial balance of 100, as long as the balance never drops below 0.");

            double startBalance = 100.0; 
  
            int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 70, 30 };

            double endBalance = attemptedWithdrawals.Aggregate(startBalance, (balance, nextWithdrawal) => ((nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance));

            Console.WriteLine("Ending balance: {0}", endBalance); 
        }
    }
}
