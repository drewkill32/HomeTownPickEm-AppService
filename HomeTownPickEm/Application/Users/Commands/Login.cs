using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Login
    {
        public class Query : IRequest<UserDto>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }


        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    throw new ForbiddenAccessException();
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return user.ToUserDto(token);
                }

                throw new ForbiddenAccessException();
            }
        }
    }
}