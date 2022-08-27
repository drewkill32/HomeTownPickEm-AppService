using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class RemoveUser
{
    public class Command : IRequest
    {
        public string UserId { get; set; }
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
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                throw new NotFoundException($"There is no user with the id {request.UserId}");
            }

            var pendingInvites = await _context.PendingInvites.Where(x => x.UserId == request.UserId)
                .ToArrayAsync(cancellationToken);

            var seasons = await _context.Season.Where(x => x.Members.Any(m => m.Id == request.UserId))
                .Include(s => s.Picks)
                .AsTracking()
                .ToArrayAsync(cancellationToken);
            foreach (var season in seasons)
            {
                season.RemoveMember(user);
            }

            _context.PendingInvites.RemoveRange(pendingInvites);

            await _context.SaveChangesAsync(cancellationToken);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Unable to delete user. {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return Unit.Value;
        }
    }
}