using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Application.Users.Commands
{
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
            private readonly UserManager<PostgresApplicationUser> _postgresUserManager;

            public Handler(UserManager<ApplicationUser> userManager, UserManager<PostgresApplicationUser> postgresUserManager,
                ILogger<Handler> logger,
                ITokenService tokenService)
            {
                _tokenService = tokenService;
                _userManager = userManager;
                _postgresUserManager = postgresUserManager;
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
                
                IdentityUser postgressUser = null;

                if (user.IsMigrated)
                {
                    postgressUser = await GetMigratedUser(user.Email, request.Password, cancellationToken);
                }
                else
                {
                    postgressUser = await MigrateUser(user, request.Password, cancellationToken);
                }
                var dto = await _tokenService.GenerateNewTokens(postgressUser.Id, cancellationToken);

                return dto;
                
            }

            private async Task<IdentityUser> MigrateUser(ApplicationUser user, string password, CancellationToken cancellationToken)
            {
                var result = await _userManager.CheckPasswordAsync(user, password);
                if (!result)
                {
                    _logger.LogWarning("User {Email} attempted to log in with an incorrect password.", user.Email);
                    throw new ForbiddenAccessException();
                }
                
                var postgressUser = await _postgresUserManager.FindByEmailAsync(user.Email);
                await RemovePassword(postgressUser);
                await UpdatePassword(postgressUser, password);
                
                user.IsMigrated = true;
                await _userManager.UpdateAsync(user);
                
                postgressUser.IsMigrated = true;
                await _postgresUserManager.UpdateAsync(postgressUser);
                
                return postgressUser;
            }

            private async Task UpdatePassword(PostgresApplicationUser postgressUser, string password)
            {
                var addPasswordResult = await _postgresUserManager.AddPasswordAsync(postgressUser, password);
                if (!addPasswordResult.Succeeded)
                {
                    _logger.LogCritical("Failed to add password for user {Email}", postgressUser.Email);
                    throw new ForbiddenAccessException();
                }
            }

            private async Task RemovePassword( PostgresApplicationUser postgressUser)
            {
                var removePasswordResult = await _postgresUserManager.RemovePasswordAsync(postgressUser);
                if (!removePasswordResult.Succeeded)
                {
                    _logger.LogCritical("Failed to remove password for user {Email}", postgressUser.Email);
                    throw new ForbiddenAccessException();
                }
            }

            private async Task<IdentityUser> GetMigratedUser(string email, string password, CancellationToken cancellationToken)
            {
                _logger.LogInformation("User {Email} is migrated to postgress. Attemping to login to postgres", email);
                var postgressUser = await _postgresUserManager.FindByEmailAsync(email);
                if (postgressUser == null)
                {
                    _logger.LogCritical("No user found with email <{email}>", email);
                    throw new ForbiddenAccessException();
                }
                var result = await _postgresUserManager.CheckPasswordAsync(postgressUser, password);
                if (!result)
                {
                    _logger.LogWarning("User {Email} attempted to log in with an incorrect password.", email);
                    throw new ForbiddenAccessException();
                }

                return postgressUser;
            }
        }
    }
}