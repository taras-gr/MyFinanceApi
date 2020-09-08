using System;

namespace MyFinance.Services.DataTransferObjects
{
    public class CategoryForCreationDto
    {
        public string Title { get; set; }

        public Guid UserId { get; set; }
    }
}