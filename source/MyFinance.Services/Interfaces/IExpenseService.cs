﻿using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId);
        Task<PagedList<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters);
        //Task<User> GetUserByName(string userName);
        //Task<User> GetUserByEmail(string email);
        //Task<List<User>> GetUsers();
        Task AddExpense(Guid userId, Expense expense);
        //Task<bool> UpdateUser(string userId, User user);
        Task DeleteUserExpenseById(Guid userId, Guid expenseId);
        Task<bool> ExpenseExistForUser(Guid userId, Guid expenseId);
    }
}