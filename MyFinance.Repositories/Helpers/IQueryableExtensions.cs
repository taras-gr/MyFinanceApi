using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace MyFinance.Repositories.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByClausesAfterSplit = orderBy.Split(',');

            var sortDescending = orderBy.EndsWith("_desc");

            foreach (var clause in orderByClausesAfterSplit.Reverse())
            {
                var indexOfFirstUnderscore = clause.IndexOf("_");
                var propertyName = clause;

                if (indexOfFirstUnderscore != -1)
                {
                    propertyName = propertyName.Remove(indexOfFirstUnderscore);
                }

                source = source.OrderBy(propertyName + (sortDescending ? " descending" : " ascending"));
            }

            return source;
        }
    }
}