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
            public int Week { get; set; }
            public string Season { get; set; }
            public string SeasonType { get; set; }
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
                var calendar = (await _context.Calendar.Where(x =>
                            x.Season == request.Season && x.SeasonType == request.SeasonType
                                                       && x.Week == request.Week)
                        .SingleOrDefaultAsync(cancellationToken))
                    .GuardAgainstNotFound();

                calendar.CutoffDate = request.CutoffDate;

                return calendar.ToCalendarDto();
            }
        }
    }
}