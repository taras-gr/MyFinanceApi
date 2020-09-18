using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Domain.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<Expense> Expenses { get; set; }
            = new List<Expense>();

        public ICollection<Category> ExpenseCategories { get; set; } 
            = new List<Category>();
    }
}
