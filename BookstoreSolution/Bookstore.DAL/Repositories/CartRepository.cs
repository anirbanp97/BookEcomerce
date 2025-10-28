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
    public class CartRepository : ICartRepository
    {
        private readonly BookstoreDbContext _context;
        public CartRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void AddToCart(int userId, int bookId, int quantity)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_AddToCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateCartItem(int userId, int bookId, int quantity)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_UpdateCartItem", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void RemoveCartItem(int userId, int bookId)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_RemoveCartItem", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@BookId", bookId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<CartItem> GetCartByUser(int userId)
        {
            var list = new List<CartItem>();
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetCartByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new CartItem
                {
                    CartItemId = (int)rdr["CartItemId"],
                    BookId = (int)rdr["BookId"],
                    Quantity = (int)rdr["Quantity"],
                    UnitPrice = (decimal)rdr["UnitPrice"],
                    BookTitle = rdr["Title"].ToString(),
                    Author = rdr["Author"].ToString()
                });
            }
            return list;
        }
    }
}
