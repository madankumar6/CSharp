using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    // Declare a delegate type for processing a book:
    public delegate void ProcessBookDelegate(Book book);

    //Describes the book
    public struct Book
    {
        public string Title;
        public string Author;
        public decimal Price;
        public bool Paperback;

        public Book(string title, string author, decimal price, bool paperback)
        {
            Title = title;
            Author = author;
            Price = price;
            Paperback = paperback;
        }
    }

    public class BookDB
    {
        ArrayList bookList = new ArrayList();

        public void AddBook(Book book)
        {
            bookList.Add(book);
        }

        public void AddBook(string title, string author, decimal price, bool paperback)
        {
            AddBook(new Book(title, author, price, paperback));
        }

        // Call a passed-in delegate on each paperback book to process it:
        public void ProcessPaperbackBooks(ProcessBookDelegate processBook)
        {
            foreach (Book book in bookList)
            {
                if (book.Paperback)
                {
                    processBook(book);
                }
            }
        }
    }

    class PriceTotaller
    {
        int countBooks = 0;
        decimal priceBooks = 0.0m;

        internal void AddBookToTotal(Book book)
        {
            countBooks++;
            priceBooks += book.Price;
        }

        internal decimal AveragePrice()
        {
            return priceBooks / countBooks;
        }
    }
}
