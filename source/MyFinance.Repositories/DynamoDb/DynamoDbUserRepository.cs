using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.DynamoDb
{
    public class DynamoDbUserRepository : IUserRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;
        private readonly Table _usersTable;

        public DynamoDbUserRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
            _usersTable = Table.LoadTable(_client, "Users");
        }

        public async Task<User> AddUser(User user)
        {
            var categoryDocument = new Document();
            categoryDocument["Id"] = user.Id;
            categoryDocument["UserName"] = user.UserName;
            categoryDocument["FirstName"] = user.FirstName;
            categoryDocument["LastName"] = user.LastName;
            categoryDocument["Email"] = user.Email;
            categoryDocument["Password"] = user.Password;

            await _usersTable.PutItemAsync(categoryDocument);
            return user;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var categories = await _context.ScanAsync<User>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Users" }).GetRemainingAsync();
            var userCategory = categories.Where(s => s.Id == userId).FirstOrDefault();

            return userCategory;
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
