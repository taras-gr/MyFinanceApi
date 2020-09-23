using System;
using System.Collections;
using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<IEnumerable> GetUserExpenseStatsByProperty(Guid userId, DateTimeOffset startDate, DateTimeOffset endDate, ExpenseGroupByProperty groupByProperty);
    }
}
