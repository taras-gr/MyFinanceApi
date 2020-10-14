using AutoMapper;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;

namespace MyFinance.Services.AutoMapperProfiles
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<ExpenseForCreationDto, Expense>();
            CreateMap<ExpenseDto, Expense>().ReverseMap();
        }
    }
}
