using System.ComponentModel.DataAnnotations.Schema;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data;

public class ApplicationDbContext : ApplicationDbContext<ApplicationUser>
{
    protected ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}

public class ApplicationDbContext<TUser> : IdentityDbContext<TUser> where TUser : IdentityUser
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

    public DbSet<WeeklyGame> WeeklyGames { get; set; }

    public DbSet<WeeklyGamePick> WeeklyGamePicks { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        //User
        var userNameBuilder = builder.Entity<ApplicationUser>()
            .OwnsOne(x => x.Name, ob =>
            {
                ob.ToTable("AspNetUsers");
                ob.Property(x => x.First).HasColumnName("FirstName").IsRequired();
                ob.Property(x => x.Last).HasColumnName("LastName");
                ob.WithOwner();
            });

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

        builder.Entity<Season>()
            .HasMany(x => x.WeeklyGames)
            .WithOne(y => y.Season)
            .HasForeignKey(x => x.SeasonId);

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

        //WeeklyGames


        builder.Entity<WeeklyGame>().HasKey(x => x.Id);
        builder.Entity<WeeklyGame>().Property(e => e.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();

        builder.Entity<WeeklyGame>()
            .HasMany(x => x.WeeklyGamePicks)
            .WithOne(y => y.WeeklyGame)
            .HasForeignKey(x => x.WeeklyGameId);

        builder.Entity<WeeklyGame>()
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(x => x.GameId);

        //WeeklyGamePick
        builder.Entity<WeeklyGamePick>().HasKey(x => x.Id);
        builder.Entity<WeeklyGamePick>().Property(e => e.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();

        builder.Entity<WeeklyGamePick>().HasOne(x => x.WeeklyGame).WithMany(x => x.WeeklyGamePicks)
            .HasForeignKey(x => x.WeeklyGameId);

        builder.Entity<WeeklyGamePick>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
    }
}