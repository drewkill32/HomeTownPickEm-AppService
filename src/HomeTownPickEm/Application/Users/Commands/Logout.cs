﻿using System.Security.Claims;
using System.Text.Json.Serialization;
using HomeTownPickEm.Application.Common;
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
        private readonly ISystemDate _date;
        private readonly ILogger<Logout> _logger;

        public CommandHandler(ITokenService tokenService, ApplicationDbContext context, ISystemDate date,
            ILogger<Logout> logger)
        {
            _tokenService = tokenService;
            _context = context;
            _date = date;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal principal;
            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error parsing access token {Token}", request.AccessToken);
                return Unit.Value;
            }

            var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var token = await _context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.RefreshTokens)
                .SelectMany(x => x.RefreshTokens)
                .Where(r => r.Token == request.RefreshToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (token != null)
            {
                _context.Remove(token);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}