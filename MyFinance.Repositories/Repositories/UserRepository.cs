using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MyFinance.Domain;
using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(IOptions<MongoDbSettings> setiings)
        {
            _context = new UserDbContext(setiings);
        }

        public async Task AddUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(ObjectId userId)
        {
            throw new NotImplementedException();
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
    }
}
