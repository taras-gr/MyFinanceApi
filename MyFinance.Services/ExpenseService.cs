using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;
using MyFinance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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

        public Task<int> Save()
        {
            throw new NotImplementedException();
        }
    }
}
