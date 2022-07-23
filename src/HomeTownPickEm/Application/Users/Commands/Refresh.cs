using System.Security.Claims;
using System.Text.Json.Serialization;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HomeTownPickEm.Application.Users.Commands;

public class Refresh
{
    public class Command : IRequest<TokenDto>
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, TokenDto>
    {
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly ISystemDate _date;
        private readonly ILogger<Refresh> _logger;

        public CommandHandler(ITokenService tokenService, ApplicationDbContext context, ISystemDate date,
            ILogger<Refresh> logger)
        {
            _tokenService = tokenService;
            _context = context;
            _date = date;
            _logger = logger;
        }

        public async Task<TokenDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var token = await _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == request.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);


            if (token == null)
            {
                throw new UnauthorizedAccessException("User or token not found");
            }

            var tokenId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (token.ExpiryDate <= _date.UtcNow || token.JwtId != tokenId)
            {
                throw new UnauthorizedAccessException("Refresh token is invalid");
            }

            var newToken = await _tokenService.GenerateNewTokens(token.UserId, cancellationToken);
            _logger.LogInformation("Removing token {Token}", token.Token);
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);
            return newToken;
        }
    }
}