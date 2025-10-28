using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Common
{
    /// <summary>
    /// Application-wide constant values.
    /// </summary>
    public static class Constants
    {
        // Application
        public const string ApplicationName = "Online Bookstore Management System";

        // Roles
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";

        // Order Status
        public const string OrderStatus_Placed = "Placed";
        public const string OrderStatus_Processing = "Processing";
        public const string OrderStatus_Shipped = "Shipped";
        public const string OrderStatus_Delivered = "Delivered";
        public const string OrderStatus_Cancelled = "Cancelled";

        // Messages
        public const string Msg_Success = "Operation completed successfully.";
        public const string Msg_Failure = "Operation failed. Please try again.";
        public const string Msg_NotFound = "Requested record not found.";
    }
}
