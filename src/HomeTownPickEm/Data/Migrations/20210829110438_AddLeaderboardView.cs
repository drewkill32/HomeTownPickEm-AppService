using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddLeaderboardView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            CREATE VIEW IF NOT EXISTS [Leaderboard]
            AS
            SELECT u.FirstName AS UserFirstName,
            u.LastName AS UserLastName, 
            t.School AS TeamSchool,
            t.Mascot AS TeamMascot,
            t.Logos AS TeamLogos,
            l.Name AS LeagueName,
            l.Slug AS LeagueSlug, 
            Sum(p.Points) AS TotalPoints
            FROM [League] l
            JOIN [ApplicationUserLeague] aul on aul.LeaguesId  = l.Id
            JOIN [AspNetUsers] u on u.id = aul.MembersId 
            JOIN [Pick] p on p.UserId = u.Id and p.LeagueId =l.Id
            JOIN [Teams] t on t.Id =u.TeamId
            ORDER BY SUM(p.Points) DESC";
 
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [Leaderboard]");
        }
    }
}
