using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;

namespace MyFinance.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userName}/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CategoryController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetUserCategories(string userName)
        {
            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;
            string currentUserName = User.Claims.First(c => c.Type == "UserName").Value;

            if(currentUserName != userName)
            {
                return Unauthorized();
            }

            var categoriesFromCategoryService = 
               await _categoryService.GetUserCategories(userIdFromToken);

            return Ok(categoriesFromCategoryService);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetUserCategoryById(Guid categoryId)
        {
            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;

            var categoryFromCategoryService =
               await _categoryService.GetUserCategoryById(userIdFromToken, categoryId.ToString());

            return Ok(categoryFromCategoryService);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategoryForUser([FromBody] CategoryForCreationDto category)
        {
            var categoryToAdd = _mapper.Map<Category>(category);

            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;

            await _categoryService.AddCategory(userIdFromToken, categoryToAdd);

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DelleteUserCategoryById(string userName, Guid categoryId)
        {
            string userIdFromToken = User.Claims.First(c => c.Type == "Id").Value;
            string currentUserName = User.Claims.First(c => c.Type == "UserName").Value;

            if (currentUserName != userName)
            {
                return Unauthorized();
            }

            var categoryToDelete = await _categoryService.GetUserCategoryById(userIdFromToken, categoryId.ToString());

            if (categoryToDelete == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteUserCategoryById(userIdFromToken, categoryId);

            return NoContent();
        }

    }
}
