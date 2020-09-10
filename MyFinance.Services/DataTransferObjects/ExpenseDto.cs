using System;

namespace MyFinance.Services.DataTransferObjects
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTimeOffset ExpenseDate { get; set; }

        public Guid UserId { get; set; }
    }
}
