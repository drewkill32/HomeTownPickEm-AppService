using System;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddLeagueCommandHandler : IRequestHandler<AddLeagueCommand, LeagueDto>
    {
        private readonly ApplicationDbContext _context;

        public AddLeagueCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeagueDto> Handle(AddLeagueCommand request, CancellationToken cancellationToken)
        {
            var league = new League
            {
                Name = request.Name,
                Season = request.Season
            };
            _context.League.Add(league);
            await _context.SaveChangesAsync(cancellationToken);
            return league.ToLeagueDto();
        }
    }

    public class AddLeagueCommand : IRequest<LeagueDto>
    {
        public string Name { get; set; }
        public string Season { get; set; } = DateTime.Now.Year.ToString();
    }
}