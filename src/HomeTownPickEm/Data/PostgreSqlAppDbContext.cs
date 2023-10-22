using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data;

public class PostgreSqlAppDbContext: ApplicationDbContext
{

    public PostgreSqlAppDbContext(DbContextOptions<PostgreSqlAppDbContext> options) : base(options)
    {
    }
    
}