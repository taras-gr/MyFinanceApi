using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using MyFinance.Services.Interfaces;

namespace MyFinance.Services
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetUserCategories(string userId)
        {
            var userIdGuid = new Guid(userId);

            return await _categoryRepository.GetUserCategories(userIdGuid);
        }

        public async Task<Category> GetUserCategoryById(string userId, string categoryId)
        {
            var userIdGuid = new Guid(userId);
            return await _categoryRepository.GetUserCategoryById(userIdGuid, new Guid(categoryId));
        }

        public async Task AddCategory(string userId, Category category)
        {
            var id = Guid.NewGuid();
            var userIdGuid = new Guid(userId);

            category.Id = id;
            category.UserId = userIdGuid;

            await _categoryRepository.AddCategory(category);
        }

        public async Task DeleteUserCategoryById(string userId, Guid categoryId)
        {
            var userIdGuid = new Guid(userId);
            await _categoryRepository.DeleteUserCategoryById(userIdGuid, categoryId);
        }        

        public async Task<bool> CategoryExistForSpecificUser(Guid userId, string categoryTitle)
        {
            var categoryFromRepo = await _categoryRepository.GetUserCategoryByTitle(userId, categoryTitle);

            return categoryFromRepo == null ? false : true;
        }
    }
}