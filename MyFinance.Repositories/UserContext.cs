using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyFinance.Domain.Models;

namespace MyFinance.Repositories
{
    public class UserDbContext
    {
        private readonly IMongoDatabase Database;

        public UserDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => Database.GetCollection<User>("Users");
    }
}
