using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class AddUserCommand : IRequest<UserDto>
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
                Name = new UserName
                {
                    First = FirstName,
                    Last = LastName
                },
                UserName = Email
            };
        }
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;


        public AddUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            return await _userManager.CreateUserAsync(request);
        }
    }
}