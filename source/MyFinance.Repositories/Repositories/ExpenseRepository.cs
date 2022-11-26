using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Linq;
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

        public async Task AddExpense(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpense(Expense expenseToEdit)
        {
            await Task.Run(() => _context.Expenses.Update(expenseToEdit));

            await _context.SaveChangesAsync();
        }

        public async Task<Expense> GetExpenseById(Guid expenseId)
        {
            var expenseToReturn = await _context.Expenses
                .FirstOrDefaultAsync(s => s.Id == expenseId);

            return expenseToReturn;
        }

        public async Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId)
        {
            var expenseToReturn = await _context.Expenses
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Id == expenseId);

            return expenseToReturn;
        }

        public async Task<IQueryable<Expense>> GetUserExpenses(Guid userId,
            ExpensesResourceParameters expensesResourceParameters)
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

            return await Task.Run(() => collection);

        }

        public async Task DeleteExpense(Expense expenseToDelete)
        {
            await Task.Run(() => _context.Expenses.Remove(expenseToDelete));

            await _context.SaveChangesAsync();
        }
    }
}
