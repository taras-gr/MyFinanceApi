using System;

namespace MyFinance.Services.DataTransferObjects
{
    public class ExpenseForCreationDto
    {
        public string Title { get; set; }

        public DateTimeOffset ExpenseDate { get; set; }

        public Guid CategoryId { get; set; }
    }
}
