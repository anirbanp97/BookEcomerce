using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        }

        public static void EnsurePositive(int value, string field)
        {
            if (value <= 0)
                throw new ArgumentException($"{field} must be greater than 0.");
        }
    }
}
