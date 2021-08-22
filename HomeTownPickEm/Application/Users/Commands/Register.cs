using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Register
    {
        public class Command : IRequest<UserDto>
        {
            public string DisplayName { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _context = context;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
                {
                    throw new BadRequestException("Email already exists");
                }

                if (await _context.Users.AnyAsync(x => x.UserName == request.Username, cancellationToken))
                {
                    throw new BadRequestException("Username already exists");
                }

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Username
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return user.ToUserDto(token);
                }

                throw new BadRequestException(string.Join(". ", result.Errors.Select(x => x.Description)));
            }
        }
    }
}