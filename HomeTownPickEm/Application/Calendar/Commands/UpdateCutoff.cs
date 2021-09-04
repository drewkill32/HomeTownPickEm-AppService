using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Calendar.Commands
{
    public class UpdateCutoff
    {
        public class Command : IRequest<CalendarDto>
        {
            public int CalendarId { get; set; }
            public DateTimeOffset? CutoffDate { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CalendarDto>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CalendarDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var calendar = (await _context.Calendar.Where(x => x.Id == request.CalendarId)
                        .SingleOrDefaultAsync(cancellationToken))
                    .GuardAgainstNotFound();

                calendar.CutoffDate = request.CutoffDate;
                _context.Update(calendar);
                await _context.SaveChangesAsync(cancellationToken);

                return calendar.ToCalendarDto();
            }
        }
    }
}