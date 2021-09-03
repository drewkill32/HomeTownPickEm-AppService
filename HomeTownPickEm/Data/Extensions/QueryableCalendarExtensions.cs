using System.Linq;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Data.Extensions
{
    public static class QueryableCalendarExtensions
    {
        public static IQueryable<Calendar> WhereWeekIs(this IQueryable<Calendar> query, int? week)
        {
            if (week.HasValue)
            {
                query = query.Where(x => x.Week == week);
            }

            return query;
        }
    }
}