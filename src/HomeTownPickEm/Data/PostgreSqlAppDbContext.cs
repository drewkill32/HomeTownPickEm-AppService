using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data;

public class PostgreSqlAppDbContext: ApplicationDbContext
{

    public DbSet<PostgresApplicationUser> PostgresUser { get; set; }
    
    public PostgreSqlAppDbContext(DbContextOptions<PostgreSqlAppDbContext> options) : base(options)
    {
    }
    
}
public class PostgreSqlIdentityAppDbContext: IdentityDbContext<PostgresApplicationUser>
{

    
    public PostgreSqlIdentityAppDbContext(DbContextOptions<PostgreSqlIdentityAppDbContext> options) : base(options)
    {
    }
    
}
