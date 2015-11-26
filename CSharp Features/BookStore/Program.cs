using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    class Program
    {
        static void PrintTitle(Book book)
        {
            Console.WriteLine(book.Title);
        }

        static void Main(string[] args)
        {
            BookDB bookDB = new BookDB();

            AddBooks(bookDB);

            Console.WriteLine("Paperback Book Titles:");
            // Create a new delegate object associated with the static 
            // method Test.PrintTitle:
            bookDB.ProcessPaperbackBooks(PrintTitle);

            // Get the average price of a paperback by using a PriceTotaller object:
            PriceTotaller totaller = new PriceTotaller();
            // Create a new delegate object associated with the nonstatic method AddBookToTotal on the object totaller:
            bookDB.ProcessPaperbackBooks(new ProcessBookDelegate(totaller.AddBookToTotal));

            Console.WriteLine("Average Paperback Book Price: ${0:#.##}",totaller.AveragePrice());
            Console.Read();
        }

        static void AddBooks(BookDB bookDB)
        {
            bookDB.AddBook("The C Programming Language",
               "Brian W. Kernighan and Dennis M. Ritchie", 19.95m, true);
            bookDB.AddBook("The Unicode Standard 2.0",
               "The Unicode Consortium", 39.95m, true);
            bookDB.AddBook("The MS-DOS Encyclopedia",
               "Ray Duncan", 129.95m, false);
            bookDB.AddBook("Dogbert's Clues for the Clueless",
               "Scott Adams", 12.00m, true);
        }
    }
}
