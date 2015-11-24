using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Ordering_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. OrderBy - Simple I \n 2. OrderBy - Simple II \n 3. OrderBy - Simple III \n 4. OrderBy - Comparer \n 5. OrderByDescending - Simple I \n 6. OrderByDescending - Simple II \n 7. OrderByDescending - Comparer \n 8. ThenBy - Simple \n 9. ThenBy - Comparer \n 10. ThenByDescending - Simple \n 11. ThenByDescending - Comparer \n 12. Reverse");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        OrderBySimpleI();
                        break;
                    case 2:
                        OrderBySimpleII();
                        break;
                    case 3:
                        OrderBySimpleIII();
                        break;
                    case 4:
                        OrderByComparer();
                        break;
                    case 5:
                        OrderByDescendingSimpleI();
                        break;
                    case 6:
                        OrderByDescendingSimpleII();
                        break;
                    case 7:
                        OrderByDescendingComparer();
                        break;
                    case 8:
                        ThenBySimple();
                        break;
                    case 9:
                        ThenByComparer();
                        break;
                    case 10:
                        ThenByDescendingSimple();
                        break;
                    case 11:
                        ThenByDescendingComparer();
                        break;
                    case 12:
                        Reverse();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void OrderBySimpleI()
        {
            Console.WriteLine("This sample uses orderby to sort a list of words alphabetically.");

            string[] words = { "cherry", "apple", "blueberry" };
            var sortedWords = from word in words
                              orderby word
                              select word;

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            } 
        }

        private static void OrderBySimpleII()
        {
            Console.WriteLine("This sample uses orderby to sort a list of words by length.");

            string[] words = { "cherry", "apple", "blueberry" };
            var sortedWords = from word in words
                              orderby word.Length
                              select word;

            Console.WriteLine("The sorted list of words (by length):");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            } 
        }

        private static void OrderBySimpleIII()
        {
            Console.WriteLine("This sample uses orderby to sort a list of products by name.");

            List<Product> products = factory.GetProductList();
            var sortedProducts = from product in products
                                 orderby product.ProductName
                                 select new { product.ProductID, product.ProductName, product.UnitPrice };

            foreach (var productInfo in sortedProducts)
            {
                Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
            }
        }

        private static void OrderByComparer()
        {
            Console.WriteLine("This sample uses an OrderBy clause with a custom comparer to do a case-insensitive sort of the words in an array.");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            var sortedWords = from word in words
                              orderby word
                              select word;

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }

            var sortedWordsCustom = words.OrderBy(word => word, new CaseInsensitiveComparer());

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWordsCustom)
            {
                Console.WriteLine(w);
            }
        }

        private static void OrderByDescendingSimpleI()
        {
            Console.WriteLine("This sample uses orderby and descending to sort a list of doubles from highest to lowest.");

            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var sortedDoubles = from doubleNumber in doubles
                                orderby doubleNumber descending
                                select doubleNumber;

            Console.WriteLine("The doubles from highest to lowest:");
            foreach (var d in sortedDoubles)
            {
                Console.WriteLine(d);
            } 

        }

        private static void OrderByDescendingSimpleII()
        {
            Console.WriteLine("This sample uses orderby to sort a list of products by units in stock from highest to lowest.");

            List<Product> products = factory.GetProductList(); 

            var sortedProducts = from product in products
                                 orderby product.ProductName descending
                                 select new { product.ProductID, product.ProductName, product.UnitPrice };

            foreach (var productInfo in sortedProducts)
            {
                Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
            }
        }

        private static void OrderByDescendingComparer()
        {
            Console.WriteLine("This sample uses an OrderBy clause with a custom comparer to do a case-insensitive descending sort of the words in an array.");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            var sortedWords = words.OrderByDescending(word => word, new CaseInsensitiveComparer());
            
            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }
        }

        private static void ThenBySimple()
        {
            Console.WriteLine("This sample uses a compound orderby to sort a list of digits, first by length of their name, and then alphabetically by the name itself.");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var sortedDigits = from digit in digits
                               orderby digit.Length, digit
                               select digit;

            Console.WriteLine("Sorted digits:");
            foreach (var d in sortedDigits)
            {
                Console.WriteLine(d);
            } 
        }

        private static void ThenByComparer()
        {
            Console.WriteLine("This sample uses an OrderBy and a ThenBy clause with a custom comparer to sort first by word length and then by a case-insensitive sort of the words in an array.");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            var sortedWords = words.OrderBy(word => word.Length).ThenBy(word => word, new CaseInsensitiveComparer());

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }
        }

        private static void ThenByDescendingSimple()
        {
            Console.WriteLine("This sample uses a compound orderby to sort a list of products, first by category, and then by unit price, from highest to lowest.");

            List<Product> products = factory.GetProductList();

            var sortedProducts = from product in products
                                 orderby product.Category, product.UnitPrice descending
                                 select product;

            foreach (var productInfo in sortedProducts)
            {
                Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductID, productInfo.ProductName, productInfo.UnitPrice);
            }

        }

        private static void ThenByDescendingComparer()
        {
            Console.WriteLine("This sample uses an OrderBy and a ThenBy clause with a custom comparer to sort first by word length and then by a case-insensitive descending sort of the words in an array.");

            string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            var sortedWords = words.OrderByDescending(word => word.Length).ThenByDescending(word => word, new CaseInsensitiveComparer());

            Console.WriteLine("The sorted list of words:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }
        }

        private static void Reverse()
        {
            Console.WriteLine("This sample uses Reverse to create a list of all digits in the array whose second letter is 'i' that is reversed from the order in the original array.");

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var reversedIDigits = (from digit in digits
                                  where digit[1] == 'i'
                                  select digit).Reverse();

            Console.WriteLine("A backwards list of the digits with a second character of 'i':");
            foreach (var d in reversedIDigits)
            {
                Console.WriteLine(d);
            }
        }
    }

    public class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
        }
    }


}
