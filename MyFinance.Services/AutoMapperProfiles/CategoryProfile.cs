using AutoMapper;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;

namespace MyFinance.Services.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryForCreationDto, Category>();
        }
    }
}