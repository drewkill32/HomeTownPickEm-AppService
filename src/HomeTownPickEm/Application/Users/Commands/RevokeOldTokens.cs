using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class RevokeOldTokens
{
    public class Command : IRequest
    {
        public string UserId { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly ISystemDate _date;

        public CommandHandler(ApplicationDbContext context, ISystemDate date)
        {
            _context = context;
            _date = date;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var tokensToRemove = (await _context.RefreshTokens
                    .Where(y => y.UserId == request.UserId)
                    .ToArrayAsync(cancellationToken))
                //sql lite can't compare dates
                .Where(x => x.ExpiryDate < _date.UtcNow.DateTime)
                .ToArray();

            foreach (var token in tokensToRemove)
            {
                _context.Remove(token);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
        }
    }
}