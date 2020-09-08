using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyFinance.Domain.Models
{
    public class ExpenseCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid UserId { get; set; }
    }
}
