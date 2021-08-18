using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class AddUser
    {
        public class Command : IRequest<UserDto>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int? TeamId { get; set; }

            public ApplicationUser ToAppUser()
            {
                return new ApplicationUser
                {
                    Email = Email,
                    TeamId = TeamId,
                    FirstName = FirstName,
                    LastName = LastName,
                    UserName = Email
                };
            }
        }

        public class CommandHandler : IRequestHandler<Command, UserDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;


            public CommandHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _userManager.CreateUserAsync(request);
            }
        }
    }
}