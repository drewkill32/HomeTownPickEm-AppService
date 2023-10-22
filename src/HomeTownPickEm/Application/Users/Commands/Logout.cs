using System.Security.Claims;
using System.Text.Json.Serialization;
using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class Logout
{
    public class Command : IRequest
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Logout> _logger;

        public CommandHandler(ITokenService tokenService, ApplicationDbContext context,
            ILogger<Logout> logger)
        {
            _tokenService = tokenService;
            _context = context;
            _logger = logger;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error parsing access token {Token}", request.AccessToken);
                return ;
            }

            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("UserId is empty");
                return;
            }

            var token = await _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == request.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (token != null)
            {
                _logger.LogInformation("Removing token {Token}", token.Token);
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync(cancellationToken);
            }

            
        }
    }
}