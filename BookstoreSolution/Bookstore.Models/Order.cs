using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Placed";
        public string? ShippingAddress { get; set; }

        // Items in the order
        public List<OrderItem> OrderItems { get; set; }
    }
}
