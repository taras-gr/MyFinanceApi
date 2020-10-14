using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;

namespace MyFinance.Repositories.Repositories
{
    public class MyFinanceContext : DbContext
    {
        public MyFinanceContext(DbContextOptions<MyFinanceContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
