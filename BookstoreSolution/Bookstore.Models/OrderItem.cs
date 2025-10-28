using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }     // DB identity
        public int OrderId { get; set; }         // FK
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Convenience properties when reading joined data
        public string? BookTitle { get; set; }
        public string? Author { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }
}
