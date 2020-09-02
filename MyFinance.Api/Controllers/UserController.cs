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

namespace MyFinance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody]UserForCreationDto user)
        {
            var userToAdd = _mapper.Map<User>(user);

            try
            {
                await _userService.AddUser(userToAdd);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", userToAdd.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.JwtSecret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]UserForLoginDto user)
        {
            var userFromRepo = await _userService.GetUserByEmail(user.Email);

            if (user != null && user.Password == userFromRepo.Password)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", userFromRepo.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.JwtSecret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }

            return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}
