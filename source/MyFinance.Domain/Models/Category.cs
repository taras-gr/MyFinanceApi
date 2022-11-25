using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinance.Domain.Models
{
    public class Category
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }
    }
}
