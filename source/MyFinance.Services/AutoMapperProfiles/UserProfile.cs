using AutoMapper;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;

namespace MyFinance.Services.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForCreationDto, User>();
        }
    }
}
