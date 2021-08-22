using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<League> League { get; set; }
        public DbSet<Pick> Pick { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Team
            builder.Entity<Team>()
                .HasKey(x => x.Id);

            builder.Entity<Team>()
                .Property(x => x.Id)
                .ValueGeneratedNever();


            //Calendar
            builder.Entity<Calendar>()
                .HasKey(x => new { x.Season, x.Week });

            //Game
            builder.Entity<Game>()
                .HasKey(x => x.Id);
            builder.Entity<Game>()
                .Property(x => x.Id)
                .ValueGeneratedNever();
            builder.Entity<Game>()
                .HasOne(x => x.Away)
                .WithMany()
                .HasForeignKey(x => x.AwayId);

            builder.Entity<Game>()
                .HasOne(x => x.Home)
                .WithMany()
                .HasForeignKey(x => x.HomeId);

            //League


            builder.Entity<League>()
                .HasIndex(x => new { x.Name, Year = x.Season })
                .IsUnique();

            //League
            builder.Entity<League>()
                .HasMany(x => x.Members)
                .WithMany(y => y.Leagues);

            builder.Entity<League>()
                .HasMany(x => x.Teams)
                .WithMany(y => y.Leagues);

            //Pick
            builder.Entity<Pick>()
                .HasOne(x => x.League)
                .WithMany(x => x.Picks)
                .HasForeignKey(x => x.LeagueId);

            builder.Entity<Pick>()
                .HasOne(x => x.Game)
                .WithMany(x => x.Picks)
                .HasForeignKey(x => x.GameId);

            builder.Entity<Pick>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<Pick>()
                .HasOne(x => x.SelectedTeam)
                .WithMany()
                .HasForeignKey(x => x.SelectedTeamId);
        }
    }
}