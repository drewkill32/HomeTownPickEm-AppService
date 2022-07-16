using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HomeTownPickEm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IWebHostEnvironment _env;

        public ApplicationDbContext(IWebHostEnvironment env,
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
            _env = env;
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<League> League { get; set; }

        public DbSet<Leaderboard> Leaderboard { get; set; }

        public DbSet<Pick> Pick { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (_env.IsDevelopment())
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var userNameBuilder = builder.Entity<ApplicationUser>()
                .OwnsOne(x => x.Name)
                .ToTable("AspNetUsers");

            userNameBuilder
                .Property(x => x.First)
                .HasColumnName("FirstName");

            userNameBuilder
                .Property(x => x.Last)
                .HasColumnName("LastName");

            //Team
            builder.Entity<Team>()
                .HasKey(x => x.Id);

            builder.Entity<Team>()
                .Property(x => x.Id)
                .ValueGeneratedNever();


            //Calendar
            builder.Entity<Calendar>()
                .HasKey(x => x.Id);

            builder.Entity<Calendar>()
                .HasOne(x => x.League)
                .WithMany()
                .HasForeignKey(x => x.LeagueId);

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

            //Leaderboard
            builder.Entity<Leaderboard>().HasNoKey();
            builder.Entity<Leaderboard>().ToView("Leaderboard");
        }
    }
}