using System;
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
            public string Email { get; set; }

            public string Password { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int? TeamId { get; set; }

            public int[] LeagueIds { get; set; } = Array.Empty<int>();
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
                if (await _context.Users.AnyAsync(x => x.UserName == request.Email, cancellationToken))
                {
                    throw new BadRequestException("Username already exists");
                }

                var leagues = await _context.League
                    .Where(x => request.LeagueIds.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                var notFoundLeagues = request.LeagueIds.Except(leagues.Select(x => x.Id)).ToArray();
                if (notFoundLeagues.Any())
                {
                    throw new NotFoundException(
                        $"leagues(s) not found with Id(s): '{string.Join(", ", notFoundLeagues.Select(x => x.ToString()))}'");
                }


                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    TeamId = request.TeamId,
                    Leagues = leagues
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var fullUser = await _context.Users
                        .Include(x => x.Team)
                        .Include(x => x.Leagues)
                        .SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
                    var token = _jwtGenerator.CreateToken(user);
                    return fullUser.ToUserDto(token);
                }

                throw new BadRequestException(string.Join(". ", result.Errors.Select(x => x.Description)));
            }
        }
    }
}