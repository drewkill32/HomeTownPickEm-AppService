using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class UpdateUser
    {
        public class Command : IRequest<UserDto>
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int? TeamId { get; set; }

            public string Email { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, UserDto>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(new[] { request.Id }, cancellationToken);
                if (user == null)
                {
                    throw new NotFoundException("User", request.Id);
                }

                UpdateApplicationUser(request, user);
                _context.Update(user);
                await _context.SaveChangesAsync(cancellationToken);
                return (await _context.Users.Include(x => x.Team)
                        .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken))
                    .ToUserDto();
            }

            private void UpdateApplicationUser(Command command, ApplicationUser user)
            {
                user.FirstName = command.FirstName ?? user.FirstName;
                user.LastName = command.LastName ?? user.LastName;
                user.TeamId = command.TeamId ?? user.TeamId;
                user.Email = command.Email ?? user.Email;
            }
        }
    }
}