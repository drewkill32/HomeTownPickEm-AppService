using System.Text;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Register
    {
        public class Command : IRequest
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Code { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly ITokenService _tokenService;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly HttpContext _httpContext;
            private readonly OriginOptions _opt;

            public Handler(ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                IOptions<OriginOptions> originOptions,
                IEmailSender emailSender,
                IHttpContextAccessor accessor,
                ITokenService tokenService)
            {
                _userManager = userManager;
                _emailSender = emailSender;
                _tokenService = tokenService;
                _context = context;
                _httpContext = accessor.HttpContext;
                _opt = originOptions.Value;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Where(x => x.Email == request.Email)
                    .AsTracking()
                    .FirstOrDefaultAsync(cancellationToken);

                if (user == null)
                {
                    throw new BadRequestException("You are not registered with this email address.");
                }

                var pendingInvites = await _context.PendingInvites
                    .Where(x => x.UserId == user.Id)
                    .ToArrayAsync(cancellationToken);

                if (!pendingInvites.Any())
                {
                    throw new UnauthorizedAccessException(
                        "You are not invited to join any leagues. Please contact your league commissioner.");
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                var result = await _userManager.ResetPasswordAsync(user, code, request.Password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(
                        $"Unable to reset password. {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                user.EmailConfirmed = true;
                foreach (var invite in pendingInvites)
                {
                    await AddInvite(invite, user, cancellationToken);
                }


                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            private async Task AddInvite(PendingInvite invite, ApplicationUser user, CancellationToken token)
            {
                var season = await _context.Season
                    .AsTracking()
                    .Where(x => x.LeagueId == invite.LeagueId && x.Year == invite.Season)
                    .Include(x => x.Teams)
                    .Include(x => x.Members)
                    .FirstOrDefaultAsync(token)
                    .GuardAgainstNotFound($"Season not found for leagueId {invite.LeagueId} and year {invite.Season}.");


                if (invite.TeamId.HasValue)
                {
                    var team = await _context.Teams
                        .Where(x => x.Id == invite.TeamId)
                        .AsTracking()
                        .FirstOrDefaultAsync(token)
                        .GuardAgainstNotFound($"Team {invite.TeamId} not found.");

                    var games = await _context.Games.AsTracking().WhereTeamIsPlaying(team).ToArrayAsync(token);
                    user.ProfileImg = team.Logos;
                    user.TeamId = team.Id;
                    season.AddTeam(team, games);
                }

                var teamIds = season.Teams.Select(x => x.Id).ToArray();
                var allGames = await _context.Games.WhereTeamsArePlaying(teamIds)
                    .AsTracking()
                    .ToArrayAsync(token);

                season.AddMember(user, allGames);
            }
        }
    }
}