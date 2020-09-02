using MongoDB.Driver;

namespace MyFinance.Repositories
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}