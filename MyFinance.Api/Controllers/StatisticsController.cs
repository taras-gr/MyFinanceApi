using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Api.Helpers;
using MyFinance.Services;
using MyFinance.Services.Interfaces;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace MyFinance.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userName}/stats")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetUserExpenseStats(
            string userName, 
            [FromQuery] DateTimeOffset startDate, 
            DateTimeOffset endDate, 
            ExpenseGroupByProperty statsByProperty)
        {
            var userIdFromToken = User.GetUserIdAsGuid();
            string currentUserName = User.GetUserName();

            if (currentUserName != userName)
            {
                return Unauthorized();
            }

            var dailyStats = await _statisticsService
                .GetUserExpenseStatsByProperty(userIdFromToken.Value, startDate, endDate, statsByProperty);

            return Ok(dailyStats);
        }
    }
}
