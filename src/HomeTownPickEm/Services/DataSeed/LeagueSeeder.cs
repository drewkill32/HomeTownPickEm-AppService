using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services.DataSeed
{
    public class LeagueSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public LeagueSeeder(ApplicationDbContext context, ILogger<LeagueSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            if (!_context.League.Any())
            {
                _context.League.Add(new League
                {
                    Name = "St. Pete Pick Em",
                    Slug = "st-pete-pick-em",
                    Season = DateTime.Now.Year.ToString()
                });
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Added Test League");
            }

            var league = await _context.League
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(x => x.Name == "St. Pete Pick Em" && string.IsNullOrEmpty(x.Slug),
                    cancellationToken);

            if (league != null)
            {
                league.Slug = "st-pete-pick-em";
                _context.Update(league);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}