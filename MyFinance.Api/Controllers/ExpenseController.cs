using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;
using System;
using System.Linq;
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

            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;

            var userId = new Guid(userIdFromToken);

            await _expenseService.AddExpense(userId, expenseToAdd);

            return Ok();
        }
    }
}
