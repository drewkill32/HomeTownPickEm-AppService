using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Login
    {
        public class Query : IRequest<TokenUserDto>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }


        public class Handler : IRequestHandler<Query, TokenUserDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly ILogger<Handler> _logger;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IMapper _mapper;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IMapper mapper,
                ILogger<Handler> logger,
                IJwtGenerator jwtGenerator)
            {
                _context = context;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _signInManager = signInManager;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<TokenUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogCritical("No user found with email <{email}>", request.Email);
                    throw new ForbiddenAccessException();
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    var claims = (await _userManager.GetClaimsAsync(user)).ToArray();
                    var fullUser = await _context.Users
                        .ProjectTo<TokenUserDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
                    fullUser.Token = _jwtGenerator.CreateToken(user, claims);
                    return fullUser;
                }

                _logger.LogWarning("User {Email} attempted to log in with an incorrect password.", request.Email);
                throw new ForbiddenAccessException();
            }
        }
    }
}