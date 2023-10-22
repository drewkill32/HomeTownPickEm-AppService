using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected ApplicationDbContext(DbContextOptions options
        ) : base(options)
        {
           
        }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
           
        }
        
        

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<Season> Season { get; set; }
        
        public DbSet<League> League { get; set; }

        public DbSet<Leaderboard> Leaderboard { get; set; }

        public DbSet<Pick> Pick { get; set; }

        public DbSet<SeasonCache> SeasonCaches { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PendingInvite> PendingInvites { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //User
            
            
            var userNameBuilder = builder.Entity<ApplicationUser>()
                .OwnsOne(x => x.Name)
                .ToTable("AspNetUsers");

            userNameBuilder
                .Property(x => x.First)
                .HasColumnName("FirstName");

            userNameBuilder
                .Property(x => x.Last)
                .HasColumnName("LastName");

            //RefreshToken
            builder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            
            //Team
            builder.Entity<Team>()
                .HasKey(x => x.Id);

            builder.Entity<Team>()
                .Property(x => x.Id)
                .ValueGeneratedNever();


            //Calendar
            builder.Entity<Calendar>()
                .HasKey(x => new { x.Week, x.Season, x.SeasonType });
            

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

            //Season

            builder.Entity<Season>()
                .HasMany(x => x.Members)
                .WithMany(y => y.Seasons);

            builder.Entity<Season>()
                .HasIndex(x => x.Year);
            
            builder.Entity<Season>()
                .HasMany(x => x.Teams)
                .WithMany(y => y.Seasons);

            //Pick
            builder.Entity<Pick>()
                .HasKey(x => x.Id);

            builder.Entity<Pick>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            
            builder.Entity<Pick>()
                .HasOne(x => x.Season)
                .WithMany(x => x.Picks)
                .HasForeignKey(x => x.SeasonId);

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


            //PendingInvites
            builder.Entity<PendingInvite>()
                .HasKey(x => new
                {
                    x.UserId,
                    x.Season,
                    x.LeagueId
                });

            //SeasonCaches
            builder.Entity<SeasonCache>()
                .HasKey(x => new
                {
                    x.Season,
                    x.Type,
                    x.Week
                });
        }
    }
}