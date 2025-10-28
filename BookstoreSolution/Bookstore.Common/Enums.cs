using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Common
{
    /// <summary>
    /// Enumerations used across layers.
    /// </summary>
    public enum OrderStatus
    {
        Placed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }

    public enum UserRole
    {
        Admin = 1,
        Customer = 2
    }

    public enum ResponseStatus
    {
        Success = 1,
        Failed = 2,
        NotFound = 3,
        Unauthorized = 4,
        ValidationError = 5
    }
}
