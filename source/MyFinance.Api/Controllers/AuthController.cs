using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using MyFinance.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using MyFinance.Services.Helpers;

namespace MyFinance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAWSCognitoService _AWSCognitoService;
        private readonly IAuthenticationManagerService _authenticationManagerService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AuthController(
            IUserService userService,
            IAWSCognitoService AWSCognitoService,
            IAuthenticationManagerService authenticationManagerService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _AWSCognitoService = AWSCognitoService;
            _authenticationManagerService = authenticationManagerService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody]UserForCreationDto user)
        {
            var signUpResult = await _AWSCognitoService.PerformSignUpAsync(user);
            var authResponse = await _AWSCognitoService.PerformAuthAsync(user);

            var userToAdd = _mapper.Map<User>(user);
            userToAdd.Id = new Guid(signUpResult.UserSub);

            try
            {
                var addedUser = await _userService.AddUser(userToAdd);

                return Ok(new { token = authResponse.AuthenticationResult.IdToken, addedUser.UserName });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]UserForLoginDto user)
        {
            var token = await _authenticationManagerService.AuthenticateAsync(user.Email, user.Password);

            if (token.Item1 != null && token.Item2 != null)
            {
                return Ok(new { token = token.Item1, userName = token.Item2 });
            }

            return BadRequest(new { message = "Username / password is incorrect." });
        }
    }
}
