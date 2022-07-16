using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class AddUsers
    {
        public class Command : List<AddUser.Command>, IRequest<IEnumerable<UserDto>>
        {
        }

        public class CommandHandler : IRequestHandler<Command, IEnumerable<UserDto>>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public CommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IEnumerable<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _userManager.CreateUsersAsync(request);
            }
        }
    }
}