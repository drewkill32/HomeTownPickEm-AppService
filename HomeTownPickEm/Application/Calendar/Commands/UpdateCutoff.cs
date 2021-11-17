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
    public class UpdateCutoffCommandHandler : IRequestHandler<UpdateCutoffCommand, CalendarDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateCutoffCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CalendarDto> Handle(UpdateCutoffCommand request, CancellationToken cancellationToken)
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

    public class UpdateCutoffCommand : IRequest<CalendarDto>
    {
        public int CalendarId { get; set; }
        public DateTimeOffset? CutoffDate { get; set; }
    }
}