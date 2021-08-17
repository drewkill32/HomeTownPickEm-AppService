using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddLeagueTeams
    {
        public class Command : IRequest<LeagueDto>
        {
        }

        public class CommandHandler : IRequestHandler<Command, LeagueDto>
        {
            public Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}