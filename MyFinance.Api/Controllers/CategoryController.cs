using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFinance.Services.Interfaces;

namespace MyFinance.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userName}/catrgories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private IUserService _userService;
        private IExpenseService _expenseService;
        private IMapper _mapper;
        private IOptions<JwtSettings> _jwtSettings;

        public CategoryController(
            IUserService userService,
            IExpenseService expenseService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _expenseService = expenseService;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }


    }
}
