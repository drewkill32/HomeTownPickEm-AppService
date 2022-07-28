using HomeTownPickEm.Data;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class UpdateLeague
    {
        public class Command : IRequest
        {
            public int LeagueId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;


            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;

            }

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
       
            }
        }
    }
}