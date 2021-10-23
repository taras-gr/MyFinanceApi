using System;

namespace MyFinance.Services.DataTransferObjects
{
    public class ExpenseForCreationDto
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public DateTime ExpenseDate { get; set; }

        public int Cost { get; set; }
    }
}
