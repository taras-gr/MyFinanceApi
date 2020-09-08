using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    }
}
