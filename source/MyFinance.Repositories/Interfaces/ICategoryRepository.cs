using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetUserCategories(Guid userId);
        Task<Category> GetUserCategoryById(Guid userId, Guid categoryId);
        Task<Category> GetUserCategoryByTitle(Guid userId, string categoryTitle);
        Task AddCategory(Guid userId, Category category);
        Task DeleteUserCategoryById(Guid userId, Guid categoryId);
        Task<int> Save();
    }
}
