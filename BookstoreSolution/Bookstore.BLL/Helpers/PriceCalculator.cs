using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Helpers
{
    public static class PriceCalculator
    {
        public static decimal CalculateCartTotal(IEnumerable<CartItem> items)
        {
            if (items == null || !items.Any())
                return 0;

            return items.Sum(i => i.UnitPrice * i.Quantity);
        }
    }
}
