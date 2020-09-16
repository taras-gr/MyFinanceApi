using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private MyFinanceContext _context;

        public ExpenseRepository(MyFinanceContext context)
        {
            _context = context;
        }

        public async Task AddExpense(Guid userId, Expense expense)
        {
            expense.UserId = userId;
            await _context.Expenses.AddAsync(expense);  
        }

        public Task<Expense> GetExpenseById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId)
        {
            var expenseToReturn = await _context.Expenses
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Id == expenseId);

            return expenseToReturn;
        }

        public async Task<PagedList<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters)
        {
            if (expensesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(ExpensesResourceParameters));
            }

            var collection = _context.Expenses as IQueryable<Expense>;

            collection = collection.Where(s => s.UserId == userId);

            if (!string.IsNullOrWhiteSpace(expensesResourceParameters.SearchQuery))
            {
                var searchQuery = expensesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(s => s.Title.Contains(searchQuery)
                || s.Category.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(expensesResourceParameters.OrderBy))
            {
                collection = collection.ApplySort(expensesResourceParameters.OrderBy);
            }

            var listToReturn = await PagedList<Expense>.Create(collection, expensesResourceParameters.PageNumber, expensesResourceParameters.PageSize);

            return listToReturn;
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
