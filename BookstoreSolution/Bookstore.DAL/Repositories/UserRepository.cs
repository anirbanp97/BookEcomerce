using Bookstore.DAL.Context;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookstoreDbContext _context;
        public UserRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public int RegisterUser(User user, string plainPassword, string roleName = "Customer")
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_RegisterUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", plainPassword);
            cmd.Parameters.AddWithValue("@RoleName", roleName);

            var outId = new SqlParameter("@OutUserId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outId);

            conn.Open();
            cmd.ExecuteNonQuery();

            return (int)outId.Value;
        }

        public User GetUserByEmail(string email)
        {
            using var conn = _context.CreateConnection();
            using var cmd = new SqlCommand("sp_GetUserByEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                return new User
                {
                    UserId = (int)rdr["UserId"],
                    UserName = rdr["UserName"].ToString(),
                    Email = rdr["Email"].ToString(),
                    RoleId = (int)rdr["RoleId"]
                };
            }
            return null;
        }
    }
}
