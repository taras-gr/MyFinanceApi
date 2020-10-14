using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Repositories
{
    public class UserRepositoryRdms : IUserRepository
    {
        private MyFinanceContext _context;

        public UserRepositoryRdms(MyFinanceContext context)
        {
            _context = context;
        }
        public async Task<User> AddUser(User user)
        {
            var result = await _context.Users.AddAsync(user);
            return result.Entity;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(s => s.Id == userId);
        }

        public Task<User> GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(string userId, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
