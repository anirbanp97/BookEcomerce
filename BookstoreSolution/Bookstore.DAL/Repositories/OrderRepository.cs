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
    public class OrderRepository : IOrderRepository
    {
        private readonly BookstoreDbContext _context;
        public OrderRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public int PlaceOrder(int userId, string shippingAddress, IEnumerable<OrderItem> items)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_PlaceOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@ShippingAddress", shippingAddress);

            // TVP DataTable
            var dt = new DataTable();
            dt.Columns.Add("BookId", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(decimal));

            foreach (var i in items)
                dt.Rows.Add(i.BookId, i.Quantity, i.UnitPrice);

            var tvpParam = cmd.Parameters.AddWithValue("@Items", dt);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.OrderItemType";

            var outParam = new SqlParameter("@OutOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outParam);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outParam.Value;
        }

        public IEnumerable<Order> GetOrdersByUser(int userId)
        {
            var list = new List<Order>();
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetOrdersByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Order
                {
                    OrderId = (int)rdr["OrderId"],
                    UserId = (int)rdr["UserId"],
                    OrderDate = (DateTime)rdr["OrderDate"],
                    TotalAmount = (decimal)rdr["TotalAmount"],
                    Status = rdr["Status"].ToString(),
                    ShippingAddress = rdr["ShippingAddress"].ToString()
                });
            }
            return list;
        }

        public Order GetOrderDetails(int orderId)
        {
            var order = new Order { OrderItems = new List<OrderItem>() };
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetOrderDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                order.OrderId = (int)rdr["OrderId"];
                order.UserId = (int)rdr["UserId"];
                order.OrderDate = (DateTime)rdr["OrderDate"];
                order.TotalAmount = (decimal)rdr["TotalAmount"];
                order.Status = rdr["Status"].ToString();
                order.ShippingAddress = rdr["ShippingAddress"].ToString();

                order.OrderItems.Add(new OrderItem
                {
                    OrderItemId = (int)rdr["OrderItemId"],
                    BookId = (int)rdr["BookId"],
                    Quantity = (int)rdr["Quantity"],
                    UnitPrice = (decimal)rdr["UnitPrice"],
                    BookTitle = rdr["Title"].ToString()
                });
            }
            return order;
        }
    }
}
