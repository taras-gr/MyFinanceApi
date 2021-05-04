using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.ResourceParameters;
using MyFinance.Services.DataTransferObjects;
using System;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId);
        Task<PagedList<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters);
        Task AddExpense(Guid userId, Expense expense);
        Task UpdateUserExpenseById(Guid userId, Guid expenseId, ExpenseForEditingDto expense);
        Task DeleteUserExpenseById(Guid userId, Guid expenseId);
        Task<bool> ExpenseExistForUser(Guid userId, Guid expenseId);
    }
}
