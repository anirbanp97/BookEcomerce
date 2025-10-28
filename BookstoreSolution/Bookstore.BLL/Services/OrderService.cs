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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public int PlaceOrder(int userId, string shippingAddress, IEnumerable<OrderItem> items)
        {
            if (items == null || !items.Any())
                throw new ArgumentException("Order must have at least one item.");

            return _orderRepository.PlaceOrder(userId, shippingAddress, items);
        }

        public IEnumerable<Order> GetOrdersByUser(int userId)
        {
            return _orderRepository.GetOrdersByUser(userId);
        }

        public Order GetOrderDetails(int orderId)
        {
            return _orderRepository.GetOrderDetails(orderId);
        }
    }
}
