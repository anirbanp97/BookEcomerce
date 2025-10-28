using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DAL.Interfaces
{
    public interface IUserRepository
    {
        int RegisterUser(User user, string plainPassword, string roleName = "Customer");
        User GetUserByEmail(string email);
    }
}
