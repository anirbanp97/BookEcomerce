using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Interfaces
{
    public interface IBookService
    {
        int AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
        Book GetBookById(int bookId);
        IEnumerable<Book> GetBooks(string search = null, int page = 1, int pageSize = 50);
    }

}
