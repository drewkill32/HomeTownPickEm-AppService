using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services.DataSeed
{
    public class CalendarSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CalendarSeeder> _logger;

        public CalendarSeeder(ApplicationDbContext context, ILogger<CalendarSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            if (!_context.Calendar.Any())
            {
                var leagueId = await _context.League.OrderBy(x => x.Id)
                    .Select(x => x.Id).FirstAsync(cancellationToken);

                // cannot group by date directly in the query due to sqlite limitations
                // see https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                var games = await _context.Games
                    .Where(x => x.Picks.Any(p => p.LeagueId == leagueId))
                    .Select(x => new
                    {
                        x.Week,
                        x.Season,
                        x.SeasonType,
                        x.StartDate
                    })
                    .ToArrayAsync(cancellationToken);

                var calendars =
                    games.GroupBy(x => new { x.Week, x.Season, x.SeasonType })
                        .Select(x => new Calendar
                        {
                            Week = x.Key.Week,
                            Season = x.Key.Season,
                            SeasonType = x.Key.SeasonType,
                            CutoffDate = x.Min(g => g.StartDate).GetLastThusMidnight(),
                            FirstGameStart = x.Min(g => g.StartDate),
                            LastGameStart = x.Max(g => g.StartDate),
                            LeagueId = leagueId
                        })
                        .OrderBy(x => x.Season)
                        .ThenBy(x => x.Week)
                        .ToArray();


                _context.Calendar.AddRange(calendars);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Added {Count} Calendar weeks", calendars.Length);
            }
        }
    }
}