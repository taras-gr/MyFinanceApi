using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense> GetExpenseById(Guid expenseId);
        Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId);
        Task<IQueryable<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters);
        Task AddExpense(Guid userId, Expense expense);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(Expense expense);
    }
}
