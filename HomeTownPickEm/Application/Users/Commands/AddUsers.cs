using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class AddUsersCommandHandler : IRequestHandler<AddUsersCommand, IEnumerable<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddUsersCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> Handle(AddUsersCommand request, CancellationToken cancellationToken)
        {
            return await _userManager.CreateUsersAsync(request);
        }
    }

    public class AddUsersCommand : List<AddUserCommand>, IRequest<IEnumerable<UserDto>>
    {
    }
}