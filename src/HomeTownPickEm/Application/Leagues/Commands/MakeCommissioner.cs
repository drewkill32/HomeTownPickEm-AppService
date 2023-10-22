using System.Security.Claims;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class MakeCommissioner
{
    public class Command : IRequest
    {
        public string MemberId { get; set; }
        public int LeagueId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MakeCommissioner> _logger;

        public Handler(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<MakeCommissioner> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.Id == request.MemberId)
                .FirstOrDefaultAsync(cancellationToken)
                .GuardAgainstNotFound("User Not Found");

            var claims = await _userManager.GetClaimsAsync(user);
            if (!claims.Any(x => x.Type == Claims.Types.Commissioner && x.Value == request.LeagueId.ToString()))
            {
                await _userManager.AddClaimAsync(user,
                    new Claim(Claims.Types.Commissioner, request.LeagueId.ToString()));
            }
            
        }
    }
}