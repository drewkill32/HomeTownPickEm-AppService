using AutoMapper;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services.CFBD;
using MediatR;

namespace HomeTownPickEm.Application.Calendar.Commands
{
    public class LoadCalendar
    {
        public class Command : IRequest<IEnumerable<CalendarDto>>
        {
            public string Year { get; set; } = DateTime.Now.Year.ToString();
        }

        public class Handler : IRequestHandler<Command, IEnumerable<CalendarDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly HttpClient _httpClient;

            public Handler(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IMapper mapper)
            {
                _httpClient = httpClientFactory.CreateClient(CfbdSettings.SettingsKey);
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CalendarDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var calendarResponse = await _httpClient.GetFromJsonAsync<IEnumerable<CalendarDto>>(
                    $"/calendar?year={request.Year}"
                    , cancellationToken);

                var calendars = _mapper.Map<IEnumerable<Models.Calendar>>(calendarResponse).ToArray();

                if (_context.Calendar.Any(c => c.Season == request.Year))
                {
                    _context.Calendar.AddOrUpdateRange(calendars, c => c.Season == request.Year);

                    var dbCalendars = _context.Calendar
                        .Where(c => c.Season == request.Year)
                        .ToArray();
                    var toDelete = dbCalendars.Except(calendars, new CalendarEqualityComparer())
                        .ToArray();
                    _context.Calendar.RemoveRange(toDelete);
            
                }
                else
                {
                    await _context.Calendar.AddRangeAsync(calendars);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return calendarResponse.ToArray();
            }
            
        }
    }
}