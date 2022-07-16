using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services
{
    public class GameTeamRepository
    {
        private readonly ApplicationDbContext _context;

        private TeamDto[] _teamCollection = Array.Empty<TeamDto>();

        public GameTeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TeamDto> TeamCollection => _teamCollection;

        public async Task<IEnumerable<TeamDto>> LoadTeamCollection(IEnumerable<Game> games,
            CancellationToken cancellationToken)
        {
            var gamesArray = games.ToArray();
            var homeIds = gamesArray.Select(x => x.HomeId);
            var awayIds = gamesArray.Select(x => x.AwayId);
            var ids = homeIds.Union(awayIds).Distinct().ToArray();
            _teamCollection = await _context.Teams
                .Where(x => ids.Contains(x.Id)).Select(x => x.ToTeamDto())
                .ToArrayAsync(cancellationToken);
            return _teamCollection;
        }

        public GameDto MapToDto(Game arg)
        {
            var game = new GameDto
            {
                Id = arg.Id,
                Season = arg.Season,
                Week = arg.Week,
                AwayPoints = arg.AwayPoints,
                HomePoints = arg.HomePoints,
                SeasonType = arg.SeasonType,
                StartDate = arg.StartDate,
                StartTimeTbd = arg.StartTimeTbd,
                HomeTeam = _teamCollection.FirstOrDefault(x => x.Id == arg.HomeId),
                AwayTeam = _teamCollection.FirstOrDefault(x => x.Id == arg.AwayId)
            };

            return game;
        }
    }
}