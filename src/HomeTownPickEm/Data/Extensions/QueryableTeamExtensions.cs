using System.Linq;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Data.Extensions
{
    public static class QueryableTeamExtensions
    {
        public static IQueryable<Team> IncludeNoConference(this IQueryable<Team> query, bool includeNoConference)
        {
            if (!includeNoConference)
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.Conference));
            }

            return query;
        }

        public static IQueryable<TRequest> Top<TRequest>(this IQueryable<TRequest> query, int? count)
        {
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query;
        }

        public static IQueryable<Team> WhereConferenceIs(this IQueryable<Team> query, string conference)
        {
            if (!string.IsNullOrEmpty(conference))
            {
                query = query.Where(x => x.Conference == conference);
            }

            return query;
        }

        public static IQueryable<Team> WhereNameLike(this IQueryable<Team> query, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var lowerName = name.ToLower();
                query = query.Where(x =>
                    x.Mascot.ToLower().Contains(lowerName) || x.School.ToLower().Contains(lowerName));
            }

            return query;
        }
    }
}