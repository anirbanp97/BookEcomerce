using Bookstore.DAL.Context;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            if (items == null) throw new ArgumentNullException(nameof(items));
            var itemList = items.ToList();
            if (!itemList.Any()) throw new ArgumentException("Order must have at least one item.", nameof(items));

            using var conn = _context.CreateConnection();
            conn.Open();

           
            using var tran = conn.BeginTransaction();
            try
            {
                
                decimal total = itemList.Sum(x => x.UnitPrice * x.Quantity);

                using (var insOrder = new SqlCommand(@"
            INSERT INTO Orders (UserId, TotalAmount, Status, ShippingAddress, OrderDate)
            VALUES (@UserId, @TotalAmount, @Status, @ShippingAddress, GETUTCDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                {
                    insOrder.Parameters.AddWithValue("@UserId", userId);
                    insOrder.Parameters.AddWithValue("@TotalAmount", total);
                    insOrder.Parameters.AddWithValue("@Status", "Placed");
                    insOrder.Parameters.AddWithValue("@ShippingAddress", shippingAddress ?? string.Empty);

                    var orderIdObj = insOrder.ExecuteScalar();
                    var orderId = Convert.ToInt32(orderIdObj);

                    using var insItem = new SqlCommand(@"
                INSERT INTO OrderItems (OrderId, BookId, Quantity, UnitPrice)
                VALUES (@OrderId, @BookId, @Quantity, @UnitPrice);", conn, tran);

                    insItem.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int));
                    insItem.Parameters.Add(new SqlParameter("@BookId", SqlDbType.Int));
                    insItem.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int));
                    insItem.Parameters.Add(new SqlParameter("@UnitPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2 });

                    foreach (var it in itemList)
                    {
                        insItem.Parameters["@OrderId"].Value = orderId;
                        insItem.Parameters["@BookId"].Value = it.BookId;
                        insItem.Parameters["@Quantity"].Value = it.Quantity;
                        insItem.Parameters["@UnitPrice"].Value = it.UnitPrice;
                        insItem.ExecuteNonQuery();
                    }

                    using var updStock = new SqlCommand(@"
                UPDATE Books
                SET StockQuantity = StockQuantity - @Qty
                WHERE BookId = @BookId;", conn, tran);

                    updStock.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Int));
                    updStock.Parameters.Add(new SqlParameter("@BookId", SqlDbType.Int));

                    foreach (var it in itemList)
                    {
                        updStock.Parameters["@Qty"].Value = it.Quantity;
                        updStock.Parameters["@BookId"].Value = it.BookId;
                        updStock.ExecuteNonQuery();
                    }

                    using var delCart = new SqlCommand(@"
                DELETE ci
                FROM CartItems ci
                JOIN Carts c ON ci.CartId = c.CartId
                WHERE c.UserId = @UserId;", conn, tran);
                    delCart.Parameters.AddWithValue("@UserId", userId);
                    delCart.ExecuteNonQuery();

                    tran.Commit();
                    return orderId;
                }
            }
            catch
            {
                try { tran.Rollback(); } catch { /* swallow rollback error */ }
                throw;
            }
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
