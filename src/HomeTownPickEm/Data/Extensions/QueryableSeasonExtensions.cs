using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data.Extensions;

public static class QueryableSeasonExtensions
{
    public static async Task<int> GetLeagueSeasonId(this IQueryable<Season> season, string year, string slug,
        CancellationToken cancellationToken)
    {
        return (await season.Where(s => s.Year == year && s.League.Slug == slug)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken))
            .GuardAgainstNotFound($"There is no season with year {year} and league {slug}");
    }
}