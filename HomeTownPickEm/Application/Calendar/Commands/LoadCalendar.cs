using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
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
            private readonly HttpClient _httpClient;

            public Handler(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
            {
                _httpClient = httpClientFactory.CreateClient(CFBDSettings.SettingsKey);
                _context = context;
            }

            public async Task<IEnumerable<CalendarDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var calendarResponse = await _httpClient.GetFromJsonAsync<IEnumerable<CalendarDto>>(
                    $"/calendar?year={request.Year}"
                    , cancellationToken);

                var calendars = calendarResponse.Select(MapToCalendar);

                if (_context.Calendar.Any())
                {
                    _context.Calendar.UpdateRange(calendars);
                }
                else
                {
                    _context.Calendar.AddRange(calendars);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return calendarResponse.ToArray();
            }

            private Models.Calendar MapToCalendar(CalendarDto dto)
            {
                return new Models.Calendar
                {
                    Season = dto.Season,
                    Week = dto.Week,
                    SeasonType = dto.SeasonType,
                    FirstGameStart = dto.FirstGameStart,
                    LastGameStart = dto.LastGameStart
                };
            }
        }
    }
}