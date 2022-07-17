using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            private readonly ApplicationDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly ILogger<Handler> _logger;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                ILogger<Handler> logger,
                IJwtGenerator jwtGenerator)
            {
                _context = context;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _signInManager = signInManager;
                _logger = logger;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogCritical("No user found with email <{email}>", request.Email);
                    throw new ForbiddenAccessException();
                }

                var claims = await _userManager.GetClaimsAsync(user);
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var fullUser = await _context.Users
                        .Include(x => x.Team)
                        .Include(x => x.Leagues)
                        .SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
                    var token = _jwtGenerator.CreateToken(user);
                    return fullUser.ToUserDto(token);
                }

                _logger.LogWarning("User {Email} attempted to log in with an incorrect password.", request.Email);
                throw new ForbiddenAccessException();
            }
        }
    }
}