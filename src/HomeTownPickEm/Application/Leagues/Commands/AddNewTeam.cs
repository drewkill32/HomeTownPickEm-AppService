using HomeTownPickEm.Data;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddNewTeam
    {
        public class Command : IRequest<LeagueDto>
        {
            public string LeagueSlug { get; set; }
            public string Season { get; set; }
            public int TeamId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, LeagueDto>
        {
            private readonly ApplicationDbContext _context;


            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            
            }

            public Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}