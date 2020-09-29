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
        Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId);
        Task<IQueryable<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters);
        //Task<User> GetUserByName(string userName);
        //Task<User> GetUserByEmail(string email);
        //Task<List<User>> GetUsers();
        Task AddExpense(Guid userId, Expense expense);
        //Task<bool> UpdateUser(string userId, User user);
        Task DeleteExpense(Expense expense);
        Task<int> Save();
    }
}
