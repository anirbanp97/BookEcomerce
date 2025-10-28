using Bookstore.BLL.Interfaces;
using Bookstore.DAL.Interfaces;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int RegisterUser(User user, string plainPassword, string roleName = "Customer")
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentException("Password is required.");

            return _userRepository.RegisterUser(user, plainPassword, roleName);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
    }
}
