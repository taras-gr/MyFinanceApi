using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userName}/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public ExpenseController(
            IUserService userService,
            IExpenseService expenseService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _expenseService = expenseService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddExpenseForUser([FromBody] ExpenseForCreationDto expense)
        {
            var expenseToAdd = _mapper.Map<Expense>(expense);

            string id = User.Claims.First(c => c.Type == "UserId").Value;

            var userId = new Guid(id);

            await _expenseService.AddExpense(userId, expenseToAdd);

            return Ok();

            //var userToAdd = _mapper.Map<User>(user);

            //try
            //{
            //    await _userService.AddUser(userToAdd);

            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new Claim[]
            //        {
            //            new Claim("UserId", userToAdd.Id.ToString())
            //        }),
            //        Expires = DateTime.UtcNow.AddDays(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.JwtSecret)), SecurityAlgorithms.HmacSha256Signature)
            //    };
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //    var token = tokenHandler.WriteToken(securityToken);
            //    return Ok(new { token });
            //}
            //catch (Exception ex)
            //{
            return StatusCode(StatusCodes.Status500InternalServerError);
            //}
        }
    }
}
