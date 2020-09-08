using MongoDB.Bson;
using MyFinance.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinance.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserById(ObjectId userId);
        Task<User> GetUserByName(string userName);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetUsers();
        Task AddUser(User user);
        Task<bool> UpdateUser(string userId, User user);
        Task<bool> DeleteUser(string userId);
        Task<int> Save();
    }
}
