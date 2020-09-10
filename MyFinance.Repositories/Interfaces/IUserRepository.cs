using MongoDB.Bson;
using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByName(string userName);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetUsers();
        Task<User> AddUser(User user);
        Task<bool> UpdateUser(string userId, User user);
        Task<bool> DeleteUser(string userId);
        Task<int> Save();
    }
}
