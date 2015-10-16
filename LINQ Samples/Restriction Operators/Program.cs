using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Restriction_Operators
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSourceFactory factory = new DataSourceFactory();

            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. Where Simple I \n 2. Where Simple II \n 3. Where Simple II \n 4. Where DrillDown \n 5. Where Indexed");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
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
        }

        public static void WhereSimple2()
        {
        }

        public static void WhereSimple3()
        {
        }

        public static void WhereDrillDown()
        {
        }

        public static void WhereIndexed()
        {
        }
    }
}
