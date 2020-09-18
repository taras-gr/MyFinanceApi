using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByName(string userName);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetUsers();
        Task<User> AddUser(User user);
        Task<bool> UpdateUser(string userId, User user);
        Task<bool> DeleteUser(string userId);
    }
}
