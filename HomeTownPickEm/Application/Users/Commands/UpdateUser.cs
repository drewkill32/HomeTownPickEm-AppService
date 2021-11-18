using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateUserCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

        private void UpdateApplicationUser(UpdateUserCommand command, ApplicationUser user)
        {
            user.Name.First = command.FirstName ?? user.Name.First;
            user.Name.Last = command.LastName ?? user.Name.Last;
            user.TeamId = command.TeamId ?? user.TeamId;
            user.Email = command.Email ?? user.Email;
        }
    }

    public class UpdateUserCommand : IRequest<UserDto>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? TeamId { get; set; }

        public string Email { get; set; }
    }
}