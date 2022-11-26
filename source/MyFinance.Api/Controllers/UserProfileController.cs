using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Api.Helpers;
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
        //private readonly IUserService _userService;
        //public UserProfileController(IUserService userManager)
        //{
        //    _userService = userManager;
        //}

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<object> GetUserProfile()
        {
            var userIdFromToken = User.GetUserIdAsGuid();

            //var user = await _userService.GetUserById(userIdFromToken);

            //return new
            //{
            //    user.UserName,
            //    user.FirstName,
            //    user.LastName,
            //    user.Email
            //};

            return new
            {
                UserName = User.Claims.FirstOrDefault(k => k.Type.Equals("name")).Value,
                FirstName = User.Claims.FirstOrDefault(k => k.Type.Contains("givenname")).Value,
                LastName = User.Claims.FirstOrDefault(k => k.Type.Contains("surname")).Value,
                Email = User.Claims.FirstOrDefault(k => k.Type.Contains("email")).Value
            };
        }
    }
}
