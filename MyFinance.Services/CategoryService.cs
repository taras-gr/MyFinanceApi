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
            var userIdGuid = new Guid(userId);
            await _categoryRepository.AddCategory(userIdGuid, category);
            await _categoryRepository.Save();
        }

        public async Task DeleteUserCategoryById(string userId, Guid categoryId)
        {
            var userIdGuid = new Guid(userId);
            await _categoryRepository.DeleteUserCategoryById(userIdGuid, categoryId);
            await _categoryRepository.Save();
        }        

        public Task<int> Save()
        {
            throw new NotImplementedException();
        }
    }
}