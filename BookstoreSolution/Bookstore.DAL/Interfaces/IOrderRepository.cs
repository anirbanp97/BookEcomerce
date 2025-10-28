using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DAL.Interfaces
{
    public interface IOrderRepository
    {
        int PlaceOrder(int userId, string shippingAddress, IEnumerable<OrderItem> items);
        IEnumerable<Order> GetOrdersByUser(int userId);
        Order GetOrderDetails(int orderId);
    }
}
