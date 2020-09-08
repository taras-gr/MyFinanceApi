using System;
using System.Threading.Tasks;
using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;
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

        public async Task AddCategory(Guid userId, ExpenseCategory expenseCategory)
        {
            await _categoryRepository.AddCategory(userId, expenseCategory);
            await _categoryRepository.Save();
        }

        public Task<ExpenseCategory> GetCategoryById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Save()
        {
            throw new NotImplementedException();
        }
    }
}