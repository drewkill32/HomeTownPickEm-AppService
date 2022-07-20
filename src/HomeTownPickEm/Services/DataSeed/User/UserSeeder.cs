using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                var registerUserCommand = new Register.Command();

                _config.GetSection("User").Bind(registerUserCommand);


                var seasonId = await _context.Season
                    .OrderBy(x => x.Id)
                    .Select(x => x.Id)
                    .SingleOrDefaultAsync(cancellationToken);

                //TODO: Add Season Command

                var teamName = _config.GetSection("User")["Team"]?.ToLower();
                //TODO: Add Team Command
                // var team = await _context.Teams
                //     .OrderBy(x => x.Id)
                //     .FirstOrDefaultAsync(x => x.School.ToLower().Contains(teamName),
                //         cancellationToken);
                //
                //
                // if (team != null)
                // {
                //     registerUserCommand.TeamId = team.Id;
                // }

                await _mediator.Send(registerUserCommand, cancellationToken);
            }
        }
    }
}