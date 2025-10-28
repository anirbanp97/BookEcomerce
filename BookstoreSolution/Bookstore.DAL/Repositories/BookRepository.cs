using Bookstore.DAL.Context;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _context;

        public BookRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public int AddBook(Book book)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_AddBook", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", book.Price);
            cmd.Parameters.AddWithValue("@StockQuantity", book.StockQuantity);

            var outId = new SqlParameter("@OutBookId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)outId.Value;
        }

        public void UpdateBook(Book book)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_UpdateBook", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", book.BookId);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", book.Price);
            cmd.Parameters.AddWithValue("@StockQuantity", book.StockQuantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteBook(int bookId)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_DeleteBook", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public Book GetBookById(int bookId)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetBookById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Book
                {
                    BookId = (int)reader["BookId"],
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"] as string,
                    Price = (decimal)reader["Price"],
                    StockQuantity = (int)reader["StockQuantity"]
                };
            }
            return null;
        }

        public IEnumerable<Book> GetBooks(string search = null, int page = 1, int pageSize = 50)
        {
            var list = new List<Book>();
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetBooks", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Search", (object)search ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PageNumber", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Book
                {
                    BookId = (int)reader["BookId"],
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"] as string,
                    Price = (decimal)reader["Price"],
                    StockQuantity = (int)reader["StockQuantity"]
                });
            }
            return list;
        }
    }
}
