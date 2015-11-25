using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Set_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Distinct - I \n 2. Distinct - II \n 3. Union - I \n 4. Union - II \n 5. Intersect - I \n 6. Intersect - II \n 7. Except - I \n 8. Except - II");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        DistinctI();
                        break;
                    case 2:
                        DistinctII();
                        break;
                    case 3:
                        UnionI();
                        break;
                    case 4:
                        UnionII();
                        break;
                    case 5:
                        IntersectI();
                        break;
                    case 6:
                        IntersectII();
                        break;
                    case 7:
                        ExceptI();
                        break;
                    case 8:
                        ExceptII();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void DistinctI()
        {
            Console.WriteLine("This sample uses Distinct to remove duplicate elements in a sequence of factors of 300.");

            int[] factorsOf300 = { 2, 2, 3, 5, 5 };

            var uniqueFactors = factorsOf300.Distinct();

            Console.WriteLine("Prime factors of 300:");
            foreach (var f in uniqueFactors)
            {
                Console.WriteLine(f);
            } 
        }

        private static void DistinctII()
        {
            Console.WriteLine("This sample uses Distinct to find the unique Category names.");

            List<Product> products = factory.GetProductList();

            var categoryNames = (from product in products
                             select product.Category).Distinct();

            Console.WriteLine("Category names:");
            foreach (var n in categoryNames)
            {
                Console.WriteLine(n);
            } 
        }

        private static void UnionI()
        {
            Console.WriteLine("This sample uses Union to create one sequence that contains the unique values from both arrays.");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var uniqueNumbers = numbersA.Union(numbersB);

            Console.WriteLine("Unique numbers from both arrays:");
            foreach (var n in uniqueNumbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void UnionII()
        {
            Console.WriteLine("This sample uses Union to create one sequence that contains the unique first letter from both product and customer names.");

            List<Product> products = factory.GetProductList();
            List<Customer> customers = factory.GetCustomerList();

            var productFirstChars = from product in products
                                    select product.ProductName[0];

            var customerFirstChars = from customer in customers
                                    select customer.CompanyName[0];

            var uniqueFirstChars = productFirstChars.Union(customerFirstChars);

            Console.WriteLine("Unique first letters from Product names and Customer names:");
            foreach (var ch in uniqueFirstChars)
            {
                Console.WriteLine(ch);
            } 
        }

        private static void IntersectI()
        {
            Console.WriteLine("This sample uses Intersect to create one sequence that contains the common values shared by both arrays.");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            var commonNumbers = numbersA.Intersect(numbersB);

            Console.WriteLine("Common numbers shared by both arrays:");
            foreach (var n in commonNumbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void IntersectII()
        {
            Console.WriteLine("This sample uses Intersect to create one sequence that contains the common first letter from both product and customer names.");

            List<Product> products = factory.GetProductList();
            List<Customer> customers = factory.GetCustomerList();

            var productFirstChars = from product in products
                                    select product.ProductName[0];

            var customerFirstChars = from customer in customers
                                     select customer.CompanyName[0];

            var commonFirstChars = productFirstChars.Intersect(customerFirstChars);

            Console.WriteLine("Unique first letters from Product names and Customer names:");
            foreach (var ch in commonFirstChars)
            {
                Console.WriteLine(ch);
            } 
        }

        private static void ExceptI()
        {
            Console.WriteLine("This sample uses Except to create a sequence that contains the values from numbersAthat are not also in numbersB.");
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            IEnumerable<int> aOnlyNumbers = numbersA.Except(numbersB);

            Console.WriteLine("Numbers in first array but not second array:");
            foreach (var n in aOnlyNumbers)
            {
                Console.WriteLine(n);
            }

            IEnumerable<int> bOnlyNumbers = numbersB.Except(numbersA);

            Console.WriteLine("Numbers in second array but not first array:");
            foreach (var n in bOnlyNumbers)
            {
                Console.WriteLine(n);
            } 
        }

        private static void ExceptII()
        {
            Console.WriteLine("This sample uses Except to create one sequence that contains the first letters of product names that are not also first letters of customer names.");

            List<Product> products = factory.GetProductList();
            List<Customer> customers = factory.GetCustomerList();

            var productFirstChars = from product in products
                                    select product.ProductName[0];

            var customerFirstChars = from customer in customers
                                     select customer.CompanyName[0];

            var productOnlyFirstChars = productFirstChars.Except(customerFirstChars);

            Console.WriteLine("First letters from Product names, but not from Customer names:");
            foreach (var ch in productOnlyFirstChars)
            {
                Console.WriteLine(ch);
            } 
        }
    }
}
