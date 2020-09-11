using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
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

        public async Task<IEnumerable<Expense>> GetUserExpenses(Guid userId)
        {
            var expensesToReturn = await _context.Expenses.Where(s => s.UserId == userId).ToListAsync();

            return expensesToReturn;
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
