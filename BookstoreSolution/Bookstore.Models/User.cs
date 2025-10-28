using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Stored as VARBINARY in DB (hashed). In model keep as byte[].
        /// If you don't load password hash into DTOs, keep this property internal to DAL.
        /// </summary>
        public byte[]? PasswordHash { get; set; }

        public int RoleId { get; set; }
    }
}
