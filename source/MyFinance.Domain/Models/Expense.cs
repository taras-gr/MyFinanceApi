using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinance.Domain.Models
{
    public class Expense
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "expenseDate")]
        public DateTime ExpenseDate { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public int Cost { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as Expense;

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
