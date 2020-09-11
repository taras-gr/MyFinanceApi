using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserProfileController(IUserService userManager)
        {
            _userService = userManager;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<object> GetUserProfile()
        {
            string id = User.Claims.First(c => c.Type == "Id").Value;

            Guid userId = new Guid(id);

            var user = await _userService.GetUserById(userId);

            return new
            {
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Email
            };
        }
    }
}
