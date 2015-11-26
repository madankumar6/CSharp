using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Join_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Cross Join \n 2. Group Join \n 3. Cross Join with Group Join \n 4. Left Outer Join");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        CrossJoin();
                        break;
                    case 2:
                        GroupJoin();
                        break;
                    case 3:
                        CrossJoinwithGroupJoin();
                        break;
                    case 4:
                        LeftOuterJoin();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void CrossJoin()
        {
            Console.WriteLine("This sample shows how to efficiently join elements of two sequences based on equality between key expressions over the two.");

            string[] categories = new string[]{ "Beverages",   
                                                "Condiments",   
                                                "Vegetables",   
                                                "Dairy Products",   
                                                "Seafood" };

            List<Product> products = factory.GetProductList();

            var crossJoin = from category in categories
                            join product in products on category equals product.Category
                            select new { Category = category, product.ProductName };

            foreach (var productCategory in crossJoin)
            {
                Console.WriteLine(productCategory.ProductName + ": " + productCategory.Category);
            } 
                            
        }

        private static void GroupJoin()
        {
            Console.WriteLine("Using a group join you can get all the products that match a given category bundled as a sequence.");

            string[] categories = new string[]{ "Beverages",   
                                                "Condiments",   
                                                "Vegetables",   
                                                "Dairy Products",   
                                                "Seafood" };

            List<Product> products = factory.GetProductList();

            var groupJoin = from category in categories
                            join product in products on category equals product.Category into productGroup
                            select new { Category = productGroup, Products = productGroup };

            foreach (var categoryGroup in groupJoin)
            {
                Console.WriteLine(categoryGroup.Category + ":");
                foreach (var p in categoryGroup.Products)
                {
                    Console.WriteLine("   " + p.ProductName);
                }
            } 
        }

        private static void CrossJoinwithGroupJoin()
        {
            Console.WriteLine("The group join operator is more general than join, as this slightly more verbose version of the cross join sample shows.");

            string[] categories = new string[]{ "Beverages",   
                                                "Condiments",   
                                                "Vegetables",   
                                                "Dairy Products",   
                                                "Seafood" };

            List<Product> products = factory.GetProductList();

            var productCrossGroupJoin = from category in categories
                            join product in products on category equals product.Category into productGroup
                            from pGroup in productGroup
                            select new { Category = pGroup.Category, pGroup.ProductName };

            foreach (var v in productCrossGroupJoin)
            {
                Console.WriteLine(v.ProductName + ": " + v.Category);
            } 
        }

        private static void LeftOuterJoin()
        {
            Console.WriteLine("A so-called outer join can be expressed with a group join. A left outer joinis like a cross join, except that all the left hand side elements get included at least once, even if they don't match any right hand side elements. Note how Vegetablesshows up in the output even though it has no matching products.");

            string[] categories = new string[]{ "Beverages",   
                                                "Condiments",   
                                                "Vegetables",   
                                                "Dairy Products",   
                                                "Seafood" };

            List<Product> products = factory.GetProductList();

            var productOuterJoin = from category in categories
                                   join product in products on category equals product.Category into productGroup
                                   from pGroup in productGroup.DefaultIfEmpty()
                                   select new { Category = category, ProductName = pGroup == null ? ("No Products") : pGroup.ProductName };

            foreach (var v in productOuterJoin)
            {
                Console.WriteLine(v.ProductName + ": " + v.Category);
            } 
        }
    }
}
