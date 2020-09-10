using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFinance.Api.Helpers;
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
        private readonly IExpenseService _expenseService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public ExpenseController(
            IExpenseService expenseService,
            ICategoryService categoryService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        [HttpGet("{expenseId}", Name = "GetUserExpenseById")]
        [Authorize]
        public async Task<ActionResult<ExpenseDto>> GetUserExpenseById(string userName, Guid expenseId)
        {
            var userIdFromToken = User.GetUserIdAsGuid();
            string currentUserName = User.GetUserName();

            if (currentUserName != userName)
            {
                return Unauthorized();
            }

            var expenseFromRepo = await _expenseService.GetUserExpenseById(userIdFromToken, expenseId);

            if(expenseFromRepo == null)
            {
                return NotFound();
            }

            return Ok(expenseFromRepo);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddExpenseForUser(string userName, [FromBody] ExpenseForCreationDto expense)
        {
            var userIdFromToken = User.GetUserIdAsGuid();
            string currentUserName = User.GetUserName();

            if (currentUserName != userName)
            {
                return Unauthorized();
            }

            var expenseEntity = _mapper.Map<Expense>(expense);
            var categoryTitleFromExpense = expenseEntity.Category;

            if(! await _categoryService
                .CategoryExistForSpecificUser(userIdFromToken, categoryTitleFromExpense))
            {
                return BadRequest();
            }

            await _expenseService.AddExpense(userIdFromToken, expenseEntity);

            var expenseToReturn = _mapper.Map<ExpenseDto>(expenseEntity);

            return CreatedAtRoute("GetUserExpenseById",
                new { userName = currentUserName, expenseId = expenseToReturn.Id },
                expenseToReturn);
        }
    }
}
