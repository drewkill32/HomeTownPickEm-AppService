using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeTownPickEm.Services.DataSeed.User
{
    public class UserSeeder : ISeeder
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UserSeeder(IConfiguration config, ApplicationDbContext context, IMediator mediator)
        {
            _config = config;
            _context = context;
            _mediator = mediator;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            if (!_context.Users.Any())
            {
                var registerUserCommand = new RegisterCommand();

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