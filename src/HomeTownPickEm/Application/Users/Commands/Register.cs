using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using HomeTownPickEm.Services;
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

            public int TeamId { get; set; }

            public int[] SeasonId { get; set; } = Array.Empty<int>();
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly ILeagueServiceFactory _leagueServiceFactory;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context,
                ILeagueServiceFactory leagueServiceFactory,
                UserManager<ApplicationUser> userManager,
                IMapper mapper,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _context = context;
                _leagueServiceFactory = leagueServiceFactory;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.UserName == request.Email, cancellationToken))
                {
                    throw new BadRequestException("Username already exists");
                }

                var leagues = await _context.Season
                    .Where(x => request.SeasonId.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                var notFoundLeagues = request.SeasonId.Except(leagues.Select(x => x.Id)).ToArray();
                if (notFoundLeagues.Any())
                {
                    throw new NotFoundException(
                        $"leagues(s) not found with Id(s): '{string.Join(", ", notFoundLeagues.Select(x => x.ToString()))}'");
                }


                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Name = new UserName
                    {
                        First = request.FirstName,
                        Last = request.LastName
                    },
                    Seasons = leagues
                };
                if (request.TeamId != 0)
                {
                    user.TeamId = request.TeamId;
                }

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    foreach (var league in leagues)
                    {
                        var leagueService = _leagueServiceFactory.Create(league.Id);
                        await leagueService.AddUserAsync(user.Id, cancellationToken);
                        if (user.TeamId.HasValue)
                        {
                            await leagueService.AddTeamAsync(user.TeamId.Value, cancellationToken);
                        }
                    }

                    var fullUser = await _context.Users
                        .ProjectTo<TokenUserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
                    fullUser.Token = _jwtGenerator.CreateToken(user);
                    return fullUser;
                }

                throw new BadRequestException(string.Join(". ", result.Errors.Select(x => x.Description)));
            }
        }
    }
}