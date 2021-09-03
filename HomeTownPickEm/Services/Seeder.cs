using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Utils;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services
{
    public class Seeder
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public Seeder(
            IMediator mediator,
            ApplicationDbContext context,
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            IServiceScopeFactory scopeFactory,
            ILogger<DatabaseInit> logger)
        {
            _mediator = mediator;
            _context = context;
            _config = config;
            _userManager = userManager;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }


        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            await AddLeagues(cancellationToken);
            await AddTeams(cancellationToken);
            await AddGames(cancellationToken);
            await AddUsers(cancellationToken);
            await AddAdminUserClaim(cancellationToken);
            await AddUserPics(cancellationToken);
            await AddCalendar(cancellationToken);
        }


        private async Task AddAdminUserClaim(CancellationToken cancellationToken)
        {
            var email = _config.GetSection("User")["Email"];
            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email == email,
                cancellationToken);
            if (user == null)
            {
                throw new NotFoundException($"There is no user with the email <{email}>");
            }

            var claims = await userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Admin"))
            {
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Admin"));
            }
        }

        private async Task AddCalendar(CancellationToken cancellationToken)
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

        private async Task AddGames(CancellationToken cancellationToken)
        {
            if (!_context.Games.Any())
            {
                await _mediator.Send(new LoadGames.Command(), cancellationToken);
                var games = await _context.Games.CountAsync(cancellationToken);
                _logger.LogInformation("Added {Count} games", games);
            }
        }

        private async ValueTask AddLeagues(CancellationToken cancellationToken)
        {
            if (!_context.League.Any())
            {
                _context.League.Add(new League
                {
                    Name = "St. Pete Pick Em",
                    Slug = "st-pete-pick-em",
                    Season = DateTime.Now.Year.ToString()
                });
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Added Test League");
            }

            var league = await _context.League
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Name == "St. Pete Pick Em" && string.IsNullOrEmpty(x.Slug),
                    cancellationToken);

            if (league != null)
            {
                league.Slug = "st-pete-pick-em";
                _context.Update(league);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task AddTeams(CancellationToken cancellationToken)
        {
            if (!_context.Teams.Any())
            {
                try
                {
                    await _mediator.Send(new LoadTeams.Command(), cancellationToken);
                    var teams = await _context.Teams.CountAsync(cancellationToken);
                    _logger.LogInformation("Added {Count} Teams", teams);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Adding Teams. {ErrorMessage}", e.Message);
                }
            }
        }

        private async Task AddUserPics(CancellationToken cancellationToken)
        {
            var usersWithoutPics = await _context.Users
                .Where(x => x.TeamId != null && string.IsNullOrEmpty(x.ProfileImg))
                .Include(x => x.Team)
                .Select(x => new { User = x, x.Team.Logos })
                .ToArrayAsync(cancellationToken);

            foreach (var userKey in usersWithoutPics)
            {
                userKey.User.ProfileImg = LogoHelper.GetSingleLogo(userKey.Logos);
                _context.Update(userKey.User);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddUsers(CancellationToken cancellationToken)
        {
            if (!_context.Users.Any())
            {
                var registerUserCommand = new Register.Command();

                _config.GetSection("User").Bind(registerUserCommand);


                var league = await _context.League.OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (league != null)
                {
                    registerUserCommand.LeagueIds = new[] { league.Id };
                }

                var teamName = _config.GetSection("User")["Team"]?.ToLower();
                var team = await _context.Teams
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync(x => x.School.ToLower().Contains(teamName),
                        cancellationToken);
                if (team != null)
                {
                    registerUserCommand.TeamId = team.Id;
                }

                await _mediator.Send(registerUserCommand, cancellationToken);
            }
        }
    }
}