using MongoDB.Driver;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using MyFinance.Services.DataTransferObjects;
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
            expense.Id = Guid.NewGuid();
            expense.UserId = userId;

            await _repository.AddExpense(expense);
        }

        public async Task UpdateUserExpenseById(Guid userId, Guid expenseId, ExpenseForEditingDto expense)
        {
            var expenseToUpdate = await GetUserExpenseById(userId, expenseId);

            expenseToUpdate.Title = expense.Title;
            expenseToUpdate.Category = expense.Category;
            expenseToUpdate.ExpenseDate = expense.ExpenseDate;
            expenseToUpdate.Cost = expense.Cost;

            await _repository.UpdateExpense(expenseToUpdate);
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

        public async Task DeleteUserExpenseById(Guid userId, Guid expenseId)
        {
            var expenseToDelete = await this.GetUserExpenseById(userId, expenseId);

            await _repository.DeleteExpense(expenseToDelete);
        }

        public Task<int> Save()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExpenseExistForUser(Guid userId, Guid expenseId)
        {
            var expense = await _repository.GetUserExpenseById(userId, expenseId);

            return expense != null;
        }
    }
}
