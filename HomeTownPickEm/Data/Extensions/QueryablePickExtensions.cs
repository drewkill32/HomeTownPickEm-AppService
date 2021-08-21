using System.Linq;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Data.Extensions
{
    public static class QueryablePickExtensions
    {
        public static IQueryable<Pick> WhereUserIs(this IQueryable<Pick> query, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            return query;
        }

        public static IQueryable<Pick> WhereWeekIs(this IQueryable<Pick> query, int? week)
        {
            if (week.HasValue)
            {
                query = query.Where(x => x.Game.Week == week);
            }

            return query;
        }
    }
}