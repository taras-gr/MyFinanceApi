using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinance.Domain.Models
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTimeOffset ExpenseDate { get; set; }

        public int Cost { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid UserId { get; set; }        
    }
}
