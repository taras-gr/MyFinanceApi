using MongoDB.Bson;
using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Domain.Models;
using MyFinance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetUserById(ObjectId userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task AddUser(User user)
        {
            await _userRepository.AddUser(user);
        }

        public Task<bool> UpdateUser(string userId, User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
