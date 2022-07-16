using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data
{
    public class DatabaseInit
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInit(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
    }
}