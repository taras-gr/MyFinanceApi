using MongoDB.Driver;

namespace MyFinance.Api
{
    public class JwtSettings
    {
        public string JwtSecret { get; set; }
    }

    public class SqlServerSettings
    {
        public string ConnectionString { get; set; }
    }
    
}
