using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Api.Helpers
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
