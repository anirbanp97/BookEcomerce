using Bookstore.BLL.Interfaces;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public int AddBook(Book book)
        {
            // Validation logic (optional)
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required.");

            if (book.Price <= 0)
                throw new ArgumentException("Book price must be greater than 0.");

            return _bookRepository.AddBook(book);
        }

        public void UpdateBook(Book book)
        {
            if (book.BookId <= 0)
                throw new ArgumentException("Invalid Book ID.");

            _bookRepository.UpdateBook(book);
        }

        public void DeleteBook(int bookId)
        {
            if (bookId <= 0)
                throw new ArgumentException("Invalid Book ID.");

            _bookRepository.DeleteBook(bookId);
        }

        public Book GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }

        public IEnumerable<Book> GetBooks(string search = null, int page = 1, int pageSize = 50)
        {
            return _bookRepository.GetBooks(search, page, pageSize);
        }
    }
}
