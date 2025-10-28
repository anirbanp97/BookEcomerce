using Bookstore.API.Models;
using Bookstore.BLL.Interfaces;
using Bookstore.Common;
using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequests model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var userId = _userService.RegisterUser(user, model.Password);
            return Ok(ApiResponse<int>.Ok(userId, "User registered successfully."));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequests model)
        {
            var user = _userService.GetUserByEmail(model.Email);
            if (user == null)
                return Unauthorized(ApiResponse<string>.Fail("Invalid email or password."));

            // (Optional) Password verification handled in service
            return Ok(ApiResponse<User>.Ok(user, "Login successful."));
        }
    }
}
