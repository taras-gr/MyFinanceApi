using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFinance.Domain.Models;

namespace MyFinance.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetUserCategories(string userId);
        Task<Category> GetUserCategoryById(string userId, string categoryId);
        Task AddCategory(string userId, Category category);
        Task DeleteUserCategoryById(string userId, Guid categoryId);
        Task<bool> CategoryExistForSpecificUser(Guid userId, string categoryTitle);
    }
}