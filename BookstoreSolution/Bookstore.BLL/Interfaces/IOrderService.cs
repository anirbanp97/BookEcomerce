using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Interfaces
{
    public interface IOrderService
    {
        int PlaceOrder(int userId, string shippingAddress, IEnumerable<OrderItem> items);
        IEnumerable<Order> GetOrdersByUser(int userId);
        Order GetOrderDetails(int orderId);
    }
}
