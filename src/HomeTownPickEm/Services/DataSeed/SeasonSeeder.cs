using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services.DataSeed;

public class SeasonSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;

    public SeasonSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed(CancellationToken cancellationToken)
    {
        var seasons = await _context.Season
            .AsTracking()
            .Include(x => x.Picks)
            .ThenInclude(p => p.Game)
            .ToArrayAsync(cancellationToken);

        foreach (var season in seasons)
        {
            var gamesToRemove = season.Picks.Select(p => p.Game)
                .Where(g => g.Season != season.Year)
                .ToArray();
            _context.Games.RemoveRange(gamesToRemove);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}