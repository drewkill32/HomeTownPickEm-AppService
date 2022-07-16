#region

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HomeTownPickEm.Services.DataSeed
{
    public class CalendarTimeZoneSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;

        public CalendarTimeZoneSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            var cals = await _context.Calendar
                .Where(x => x.CutoffDate != null)
                .AsTracking()
                .ToArrayAsync(cancellationToken);

            var cutOffAtMidnightUTC =
                cals.Where(x => x.CutoffDate.Value.TimeOfDay == new TimeSpan(0, 0, 0, 0)).ToArray();

            if (cutOffAtMidnightUTC.Length == 0)
            {
                return;
            }

            var timeZoneinfo = TimeZoneInfo.Local;
            foreach (var calendar in cutOffAtMidnightUTC)
            {
                var offset = timeZoneinfo.GetUtcOffset(calendar.CutoffDate.Value).Multiply(-1);
                var newTime = calendar.CutoffDate.Value.Add(offset);
                calendar.CutoffDate = newTime;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}