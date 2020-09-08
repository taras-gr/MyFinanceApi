﻿using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> GetExpenseById(Guid userId);
        //Task<User> GetUserByName(string userName);
        //Task<User> GetUserByEmail(string email);
        //Task<List<User>> GetUsers();
        Task AddExpense(Guid userId, Expense expense);
        //Task<bool> UpdateUser(string userId, User user);
        //Task<bool> DeleteUser(string userId);
        Task<int> Save();
    }
}