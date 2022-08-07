using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class RemovePendingMember
{
    public class Command : ILeagueCommissionerRequest
    {
        public int LeagueId { get; set; }
        public string Season { get; set; }
        public string MemberId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Handler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var pendingInvites = await _context.PendingInvites
                .AsTracking()
                .Where(x => x.LeagueId == request.LeagueId
                            && x.Season == request.Season
                            && x.UserId == request.MemberId)
                .ToArrayAsync(cancellationToken);

            foreach (var pendingInvite in pendingInvites)
            {
                _context.PendingInvites.Remove(pendingInvite);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var user = await _userManager.FindByIdAsync(request.MemberId);

            if (user == null)
            {
                throw new BadRequestException($"no user found with Id: '{request.MemberId}'.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Unknown error occurred while removing user. {string.Join(", ", result.Errors.Select(x => x.Description).ToArray())}");
            }

            return Unit.Value;
        }
    }
}