using Microsoft.EntityFrameworkCore;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using MyFinance.Services.Interfaces;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Services
{
    public enum ExpenseGroupByProperty
    {
        ExpenseDate,
        Category,
        Title
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly IExpenseRepository _expenseRepository;

        public StatisticsService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable> GetUserExpenseStatsByProperty(
            Guid userId, 
            DateTimeOffset startDate, 
            DateTimeOffset endDate, 
            ExpenseGroupByProperty groupByProperty)
        {
            var expensesResourceParameters = new ExpensesResourceParameters();          

            var expensesCollection = await _expenseRepository
                .GetUserExpenses(userId, expensesResourceParameters);            

            switch (groupByProperty)
            {
                case ExpenseGroupByProperty.ExpenseDate:
                    var queryByExpenseDate = expensesCollection
                        .Where(s => s.ExpenseDate >= startDate && s.ExpenseDate <= endDate)
                        .OrderBy(o => o.ExpenseDate)
                        .GroupBy(
                            exp => exp.ExpenseDate,
                            exp => exp.Cost,
                            (date, costs) => new
                            {
                                Day = date.ToString("d"),
                                Costs = costs.Sum()
                            });

                    return await Task.FromResult(queryByExpenseDate.ToList());
                case ExpenseGroupByProperty.Category:
                    var queryByCategory = expensesCollection
                        .Where(s => s.ExpenseDate >= startDate && s.ExpenseDate <= endDate)
                        .GroupBy(
                            exp => exp.Category,
                            exp => exp.Cost,
                            (date, costs) => new
                            {
                                Category = date,
                                Costs = costs.Sum()
                            });

                    return await Task.FromResult(queryByCategory.ToList());
                case ExpenseGroupByProperty.Title:
                    var queryByTitle = expensesCollection
                        .Where(s => s.ExpenseDate >= startDate && s.ExpenseDate <= endDate)
                        .GroupBy(
                            exp => exp.Title,
                            exp => exp.Cost,
                            (date, costs) => new
                            {
                                Title = date,
                                Costs = costs.Sum()
                            });

                    return await Task.FromResult(queryByTitle.ToList());
                default:
                    throw new ArgumentException();
            }
        }
    }
}