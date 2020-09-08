using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;

namespace MyFinance.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userName}/catrgories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private IUserService _userService;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        private IOptions<JwtSettings> _jwtSettings;

        public CategoryController(
            IUserService userService,
            ICategoryService categoryService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _categoryService = categoryService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public async Task<ActionResult> AddCategoryForUser([FromBody] CategoryForCreationDto category)
        {
            var categoryToAdd = _mapper.Map<ExpenseCategory>(category);

            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;

            var userId = new Guid(userIdFromToken);

            await _categoryService.AddCategory(userId, categoryToAdd);

            return Ok();
        }

    }
}
