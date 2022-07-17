using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Calendar.Commands;
using HomeTownPickEm.Data;
using MediatR;

namespace HomeTownPickEm.Services.DataSeed
{
    public class CalendarSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<CalendarSeeder> _logger;

        public CalendarSeeder(ApplicationDbContext context, IMediator mediator, ILogger<CalendarSeeder> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year.ToString();
            if (!_context.Calendar.Any(c => c.Season == year))
            {
                var calendars = (await _mediator.Send(new LoadCalendar.Command
                {
                    Year = year
                }, cancellationToken)).ToArray();
                
                
                _logger.LogInformation("Added {Count} Calendar weeks", calendars.Length);
            }
        }
    }
}