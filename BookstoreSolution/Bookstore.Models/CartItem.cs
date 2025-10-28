using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }

        // Quantity in cart
        public int Quantity { get; set; }

        // Unit price captured at time of adding to cart
        public decimal UnitPrice { get; set; }

        // Convenience/read-only properties (filled by repository when selecting joined data)
        public string? BookTitle { get; set; }
        public string? Author { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;
    }
}
