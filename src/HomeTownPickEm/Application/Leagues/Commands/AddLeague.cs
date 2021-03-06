using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddLeague
    {
        public class Command : IRequest<LeagueDto>
        {
            public string Name { get; set; }
            public string Season { get; set; } = DateTime.Now.Year.ToString();
            public string Slug { get; set; }
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
                var league = new League
                {
                    Name = request.Name,
                    Slug = request.Slug,
                    Seasons =
                    {
                        new Season
                        {
                            Year = request.Season
                        }
                    }
                };
                
                _context.League.Add(league);
                await _context.SaveChangesAsync(cancellationToken);
                return league.ToLeagueDto();
            }
        }
    }
}