using System;
using System.Threading.Tasks;
using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;

namespace MyFinance.Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private MyFinanceContext _context;

        public CategoryRepository(MyFinanceContext context)
        {
            _context = context;
        }

        public async Task AddCategory(Guid userId, ExpenseCategory expenseCategory)
        {
            expenseCategory.UserId = userId;
            await _context.AddAsync(expenseCategory);
        }

        public Task<ExpenseCategory> GetCategoryById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}