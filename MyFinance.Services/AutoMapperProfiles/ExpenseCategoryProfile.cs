using AutoMapper;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;

namespace MyFinance.Services.AutoMapperProfiles
{
    public class ExpenseCategoryProfile : Profile
    {
        public ExpenseCategoryProfile()
        {
            CreateMap<CategoryForCreationDto, ExpenseCategory>();
        }
    }
}