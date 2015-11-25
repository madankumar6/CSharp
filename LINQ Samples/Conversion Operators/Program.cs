using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataSource;

namespace Conversion_Operators
{
    class Program
    {
        static DataSourceFactory factory = new DataSourceFactory();

        static void Main(string[] args)
        {
            int choice = 0;

            do
            {
                Console.WriteLine("Please choose the Linq Sample \n 0. Exit the Application \n 1. ToArray \n 2. ToList \n 3. ToDictionary \n 4. OfType");
                Console.Write("Enter your choice : ");
                choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        ToArray();
                        break;
                    case 2:
                        ToList();
                        break;
                    case 3:
                        ToDictionary();
                        break;
                    case 4:
                        OfType();
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again");
                        break;
                }

            } while (choice != 0);
        }

        private static void ToArray()
        {
            Console.WriteLine("This sample uses ToArray to immediately evaluate a sequence into an array.");

            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            var sortedDoubles = from doubleNum in doubles
                                orderby doubleNum descending
                                select doubleNum;

            var doublesArray = sortedDoubles.ToArray();

            Console.WriteLine("Every other double from highest to lowest:");
            for (int d = 0; d < doublesArray.Length; d += 1)
            {
                Console.WriteLine(doublesArray[d]);
            } 
        }

        private static void ToList()
        {
            Console.WriteLine("This sample uses ToList to immediately evaluate a sequence into a List<T>.");

            string[] words = { "cherry", "apple", "blueberry" };

            var sortedWords = from word in words
                              orderby word 
                              select word;
            var wordList = sortedWords.ToList();

            Console.WriteLine("The sorted word list:");
            foreach (var w in wordList)
            {
                Console.WriteLine(w);
            } 


        }

        private static void ToDictionary()
        {
            Console.WriteLine("This sample uses ToDictionary to immediately evaluate a sequence and a related key expression into a dictionary.");

            var scoreRecords = new[] { new {Name = "Alice", Score = 50}, 
                                new {Name = "Bob"  , Score = 40}, 
                                new {Name = "Cathy", Score = 45} 
                            };

            var scoreRecordsDict = scoreRecords.ToDictionary(scoreRecord => scoreRecord.Name);

            foreach (var item in scoreRecordsDict)
            {
             Console.WriteLine("Name = {0}, Score = {1}",item.Key, item.Value);   
            }

            Console.WriteLine("Bob's score: {0}", scoreRecordsDict["Bob"]); 
        }

        private static void OfType()
        {
            Console.WriteLine("This sample uses OfType to return only the elements of the array that are of type double.");

            object[] numbers = {null, 1.0, "two", 3, "four", 5, "six", 7.0 }; 

            //var doubles = from number in numbers
            //              where number.GetType() == typeof(double)
            //              select number;

            var doubles = numbers.OfType<double>();

            Console.WriteLine("Numbers stored as doubles:");
            foreach (var d in doubles)
            {
                Console.WriteLine(d);
            } 
        }
    }
}
