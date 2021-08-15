using HomeTownPickEm.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>()
                .HasKey(x => x.Id);
            builder.Entity<Team>()
                .Property(x => x.Id)
                .ValueGeneratedNever();


            builder.Entity<Calendar>()
                .HasKey(x => new { x.Season, x.Week });


            builder.Entity<Game>()
                .HasKey(x => x.Id);
            builder.Entity<Game>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Entity<Game>()
                .HasOne(x => x.Away)
                .WithMany();
            builder.Entity<Game>()
                .HasOne(x => x.Home)
                .WithMany();
        }
    }
}