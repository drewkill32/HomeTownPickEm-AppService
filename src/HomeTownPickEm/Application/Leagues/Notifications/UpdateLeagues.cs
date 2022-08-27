using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Notifications;

public class UpdateLeagues : INotificationHandler<GamesUpdatedNotification>
{
    private readonly ApplicationDbContext _context;
    private readonly ISystemDate _date;


    public UpdateLeagues(ApplicationDbContext context, ISystemDate date)
    {
        _context = context;
        _date = date;
    }


    private async Task<int> GetWeek(int? requestWeek, CancellationToken token)
    {
        if (requestWeek.HasValue)
        {
            return requestWeek.Value;
        }

        var year = _date.Year;
        var cal = await _context.Calendar.Where(x => x.Season == year).ToArrayAsync(token);
        var currWeek =
            cal.FirstOrDefault(x => x.FirstGameStart >= _date.UtcNow && x.LastGameStart <= _date.UtcNow);

        return currWeek?.Week ?? -1;
    }

    public async Task Handle(GamesUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var week = await GetWeek(notification.Week, cancellationToken);

        var gamesQuery = _context.Games
            .Where(x => x.Season == notification.Year);

        if (week != -1)
        {
            gamesQuery = gamesQuery.Where(x => x.Week == week);
        }


        var seasons = await _context.Season
            .Where(x => x.Year == notification.Year)
            .Include(s => s.Teams)
            .AsTracking()
            .ToArrayAsync(cancellationToken);

        foreach (var season in seasons)
        {
            var games = await gamesQuery
                .WhereTeamsArePlaying(season.Teams)
                .ToArrayAsync(cancellationToken);
            var gameIds = games.Select(g => g.Id).ToArray();

            //load the picks for ef tracking
            await _context.Pick
                .Where(p => p.SeasonId == season.Id && gameIds.Contains(p.GameId))
                .AsTracking()
                .ToArrayAsync(cancellationToken);

            season.UpdatePicks(games);
        }


        await _context.SaveChangesAsync(cancellationToken);
    }
}