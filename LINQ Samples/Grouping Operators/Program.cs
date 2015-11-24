using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Grouping_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. GroupBy - Simple I \n 2. GroupBy - Simple II \n 3. GroupBy - Simple III \n 4. GroupBy - Nested \n 5. GroupBy - Comparer \n 6. GroupBy - Comparer, Mapped \n 7. OrderByDescending - Comparer \n 8. ThenBy - Simple \n 9. ThenBy - Comparer \n 10. ThenByDescending - Simple \n 11. ThenByDescending - Comparer \n 12. Reverse");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        GroupBySimpleI();
                        break;
                    case 2:
                        GroupBySimpleII();
                        break;
                    case 3:
                        GroupBySimpleIII();
                        break;
                    case 4:
                        GroupByNested();
                        break;
                    case 5:
                        GroupByComparer();
                        break;
                    case 6:
                        GroupByComparerMapped();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void GroupBySimpleI()
        {
            Console.WriteLine("This sample uses group by to partition a list of numbers by their remainder when divided by 5.");

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var numberGroups = from number in numbers
                               group number by number % 5 into numberGroup
                               select new { Remainder = numberGroup.Key, Numbers = numberGroup };

            foreach (var g in numberGroups)
            {
                Console.WriteLine("Numbers with a remainder of {0} when divided by 5:", g.Remainder);
                foreach (var n in g.Numbers)
                {
                    Console.WriteLine(n);
                }
            } 
        }

        private static void GroupBySimpleII()
        {
            Console.WriteLine("This sample uses group by to partition a list of words by their first letter.");

            string[] words = { "blueberry", "chimpanzee", "abacus", "banana", "apple", "cheese" };
            var wordGroups = from word in words
                             group word by word[0] into wordGroup
                             select new { FirstLetter = wordGroup.Key, Words = wordGroup };

            foreach (var g in wordGroups)
            {
                Console.WriteLine("Words that start with the letter '{0}':", g.FirstLetter);
                foreach (var w in g.Words)
                {
                    Console.WriteLine(w);
                }
            } 
        }

        private static void GroupBySimpleIII()
        {
            Console.WriteLine("This sample uses group by to partition a list of products by category.");

            List<Product> products = factory.GetProductList();

            var orderGroups = from product in products
                              group product by product.Category into productGroup
                              select new { Category = productGroup.Key, Products = productGroup };

            foreach (var item in orderGroups)
            {
                Console.WriteLine("Categoey = {0}, Products : ", item.Category);

                foreach (var productInfo in item.Products)
                {
                    Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
                }
            }
        }

        private static void GroupByNested()
        {
            Console.WriteLine("This sample uses group by to partition a list of each customer's orders, first by year, and then by month.");
            
            List<Customer> customers = factory.GetCustomerList();

            var customerOrderGroups = from customer in customers
                                      select
                                      new
                                      {
                                          customer.CompanyName,
                                          YearGroups = from order in customer.Orders
                                                       group order by order.OrderDate.Year into yearGroup
                                                       select
                                                       new
                                                       {
                                                           Year = yearGroup.Key,
                                                           MonthGroups = from order in yearGroup
                                                                         group order by order.OrderDate.Month into monthGroup
                                                                         select
                                                                         new 
                                                                         {
                                                                             Month = monthGroup.Key,
                                                                             Orders = monthGroup
                                                                         }

                                                       }
                                      };

            foreach (var item in customerOrderGroups)
            {
                Console.WriteLine("Company Name = {0}, Year Groups ", item.CompanyName);

                foreach (var yearGroup in item.YearGroups)
                {
                    Console.WriteLine("Year : {0}", yearGroup.Year);

                    foreach (var monthGroup in yearGroup.MonthGroups)
                    {
                        Console.WriteLine("Month : {0}", monthGroup.Month);

                        foreach (var monthOrders in monthGroup.Orders)
                        {
                            Console.WriteLine("Order ID : = {0}, Total = {1} ",monthOrders.OrderID, monthOrders.Total);
                        }
                    }
                }
            }
        }

        private static void GroupByComparer()
        {
            Console.WriteLine("This sample uses GroupBy to partition trimmed elements of an array using a custom comparer that matches words that are anagrams of each other.");
            
            string[] anagrams = { "from   ", " salt", " earn ", "  last   ", " near ", " form  " };

            var orderGroups = anagrams.GroupBy(word => word.Trim(), new AnagramEqualityComparer());

            Console.WriteLine("Anagrams");
        }

        private static void GroupByComparerMapped()
        {
            Console.WriteLine("");
        }
    }

    public class AnagramEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return getCanonicalString(x) == getCanonicalString(y);
        }

        public int GetHashCode(string obj)
        {
            return getCanonicalString(obj).GetHashCode();
        }

        private string getCanonicalString(string word)
        {
            char[] wordChars = word.ToCharArray();
            Array.Sort<char>(wordChars);
            return new string(wordChars);
        }
    }
}
