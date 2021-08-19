using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class UpdateLeague
    {
        public class Command : IRequest<LeagueDto>
        {
            public string Name { get; set; }
            public string Season { get; set; }
            public int[] TeamIds { get; set; } = Array.Empty<int>();
            public string[] MemberIds { get; set; } = Array.Empty<string>();
        }

        public class CommandHandler : IRequestHandler<Command, LeagueDto>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var members = await GetMembers(request, cancellationToken);
                var teams = await GetTeams(request, cancellationToken);


                var league =
                    await _context.League.Where(x => x.Name == request.Name && x.Season == request.Season)
                        .Include(x => x.Teams)
                        .Include(x => x.Members)
                        .AsTracking()
                        .AsSingleQuery()
                        .SingleOrDefaultAsync(cancellationToken);

                if (league == null)
                {
                    throw new NotFoundException(
                        $"No League found with name {request.Name} and Season {request.Season}");
                }

                foreach (var team in teams)
                {
                    league.Teams.Add(team);
                }

                foreach (var member in members)
                {
                    league.Members.Add(member);
                }

                _context.League.Update(league);

                await _context.SaveChangesAsync(cancellationToken);

                return league.ToLeagueDto();
            }

            private async Task<ApplicationUser[]> GetMembers(Command request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.Where(x => request.MemberIds.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                var notFoundMembers = request.MemberIds.Except(users.Select(x => x.Id)).ToArray();
                if (notFoundMembers.Any())
                {
                    throw new NotFoundException(
                        $"member(s) not found with Id(s): '{string.Join(", ", notFoundMembers)}'");
                }

                return users;
            }

            private async Task<Team[]> GetTeams(Command request, CancellationToken cancellationToken)
            {
                var teams = await _context.Teams.Where(x => request.TeamIds.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                var notFoundTeams = request.TeamIds.Except(teams.Select(x => x.Id)).ToArray();
                if (notFoundTeams.Any())
                {
                    throw new NotFoundException(
                        $"team(s) not found with Id(s): '{string.Join(", ", notFoundTeams.Select(x => x.ToString()))}'");
                }

                return teams;
            }
        }
    }
}