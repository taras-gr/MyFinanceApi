using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Api.Helpers;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.ResourceParameters;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
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

        public ExpenseController(
            IExpenseService expenseService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _mapper = mapper;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedList<Expense>>> GetUserExpenses(string userName, 
            [FromQuery] ExpensesResourceParameters expensesResourceParameters)
        {
            var userIdFromToken = User.GetUserIdAsGuid();
            string currentUserName = User.GetUserName();

            if (currentUserName != userName)
            {
                return Unauthorized();
            }

            var expensesFromRepo = await _expenseService.GetUserExpenses(userIdFromToken, expensesResourceParameters);

            var paginationMetaData = new
            {
                totalCount = expensesFromRepo.TotalCount,
                pageSize = expensesFromRepo.PageSize,
                currentPage = expensesFromRepo.CurrentPage,
                totalPages = expensesFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetaData));

            var expenses = _mapper.Map<IEnumerable<ExpenseDto>>(expensesFromRepo);

            return Ok(expenses);
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
