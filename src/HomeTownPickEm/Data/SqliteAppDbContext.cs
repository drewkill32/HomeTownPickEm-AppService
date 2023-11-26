using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data;

public class SqliteAppDbContext: ApplicationDbContext
{

    public SqliteAppDbContext(DbContextOptions<SqliteAppDbContext> options) : base(options)
    {
    }
    
}