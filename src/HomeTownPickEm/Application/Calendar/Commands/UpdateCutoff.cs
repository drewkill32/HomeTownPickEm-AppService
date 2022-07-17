using HomeTownPickEm.Data;
using MediatR;

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

            public Task<CalendarDto> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}