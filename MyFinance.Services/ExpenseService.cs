using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using MyFinance.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyFinance.Services
{
    public class ExpenseService : IExpenseService
    {
        private IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task AddExpense(Guid userId, Expense expense)
        {
            await _repository.AddExpense(userId, expense);
            await _repository.Save();
        }

        public Task<Expense> GetExpenseById(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId)
        {
            return await _repository.GetUserExpenseById(userId, expenseId);
        }

        public async Task<PagedList<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters)
        {
            var collection = await _repository.GetUserExpenses(userId, expensesResourceParameters);

            var listToReturn = await PagedList<Expense>.Create(collection, expensesResourceParameters.PageNumber, expensesResourceParameters.PageSize);

            return listToReturn;
        }

        public Task<int> Save()
        {
            throw new NotImplementedException();
        }
    }
}
