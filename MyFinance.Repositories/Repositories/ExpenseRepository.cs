using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
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

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
