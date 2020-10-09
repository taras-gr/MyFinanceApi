using AutoMapper.Configuration.Conventions;
using System;

namespace MyFinance.Services.DataTransferObjects
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public DateTimeOffset ExpenseDate { get; set; }

        public int Cost { get; set; }

        public Guid UserId { get; set; }

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as ExpenseDto;

            if (Id == objectToCompare.Id &&
                Title == objectToCompare.Title &&
                Category == objectToCompare.Category &&
                ExpenseDate == objectToCompare.ExpenseDate &&
                Cost == objectToCompare.Cost &&
                UserId == objectToCompare.UserId)
                return true;

            return base.Equals(obj);
        }
    }
}
