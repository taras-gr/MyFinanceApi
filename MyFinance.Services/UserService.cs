using MongoDB.Bson;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
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

        public async Task<User> GetUserById(ObjectId userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public Task<User> GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddUser(User user)
        {
            var result = await _userRepository.AddUser(user);
            await _userRepository.Save();
            return result;
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
