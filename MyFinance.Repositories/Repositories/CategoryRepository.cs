using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;

namespace MyFinance.Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private MyFinanceContext _context;

        public CategoryRepository(MyFinanceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetUserCategories(Guid userId)
        {
            var categoriesToReturn = await _context.Categories
                .Where(s => s.UserId == userId).ToListAsync();

            return categoriesToReturn;
        }

        public async Task<Category> GetUserCategoryById(Guid userId, Guid categoryId)
        {
            var categoryToReturn = await _context.Categories
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Id == categoryId);

            return categoryToReturn;
        }

        public async Task AddCategory(Guid userId, Category category)
        {
            category.UserId = userId;

            await _context.Categories.AddAsync(category);
        }

        public async Task DeleteUserCategoryById(Guid userId, Guid categoryId)
        {
            var categoryToDelete = await _context.Categories
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Id == categoryId);

            _context.Categories.Remove(categoryToDelete);
        }      

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}