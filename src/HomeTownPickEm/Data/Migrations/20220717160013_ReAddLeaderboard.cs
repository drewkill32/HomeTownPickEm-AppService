using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class ReAddLeaderboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                DROP VIEW IF EXISTS [Leaderboard];
                CREATE VIEW IF NOT EXISTS [Leaderboard]
                AS
                SELECT 
                    u.Id AS UserId,
                    u.FirstName AS UserFirstName,
                    u.LastName AS UserLastName,
                    t.Id AS TeamId, 
                    t.School AS TeamSchool,
                    t.Mascot AS TeamMascot,
                    t.Logos AS TeamLogos,
                    l.Id AS LeagueId,
                    l.Name AS LeagueName,
                    l.Slug AS LeagueSlug, 
                    s.'Year',
                    Sum(p.Points) AS TotalPoints
                FROM [Season] s
                JOIN [League] l on s.LeagueId = l.Id 
                JOIN [ApplicationUserSeason] aus on aus.SeasonsId  = s.Id
                JOIN [AspNetUsers] u on u.id = aus.MembersId 
                JOIN [Teams] t on t.Id =u.TeamId
                JOIN [Pick] p on p.UserId = u.Id and p.SeasonId  =s.Id
                GROUP BY
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    t.Id,
                    t.School,
                    t.Mascot,
                    t.Logos,
                    l.Id,
                    l.Name,
                    l.Slug,
                    s.'Year'";
 
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [Leaderboard]");
        }
    }
}
