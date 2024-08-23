using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands;

public class Login
{
    public class Query : IRequest<TokenDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }


    public class Handler : IRequestHandler<Query, TokenDto>
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<Handler> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public Handler(UserManager<ApplicationUser> userManager, UserManager<ApplicationUser> postgresUserManager,
            ILogger<Handler> logger,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<TokenDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogCritical("No user found with email <{email}>", request.Email);
                throw new ForbiddenAccessException();
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (result)
            {
                var dto = await _tokenService.GenerateNewTokens(user.Id, cancellationToken);

                return dto;
            }

            _logger.LogWarning("User {Email} attempted to log in with an incorrect password.", request.Email);
            throw new ForbiddenAccessException();
        }
    }
}