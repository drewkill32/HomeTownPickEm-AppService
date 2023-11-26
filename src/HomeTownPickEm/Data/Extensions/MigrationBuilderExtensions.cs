using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.Extensions.Options;

namespace HomeTownPickEm.Data.Extensions;

public static class MigrationBuilderExtensions
{
    public static OperationBuilder<SqlOperation> DropLeaderboardView(this MigrationBuilder migrationBuilder)
    {
        return migrationBuilder.Sql("DROP VIEW IF EXISTS [Leaderboard];");
    }
    
    public static OperationBuilder<SqlOperation> AddLeaderboardView(this MigrationBuilder migrationBuilder)
    {
        var sql = @"
            DROP VIEW IF EXISTS [Leaderboard];
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
            JOIN [Teams] t on t.Id =u.TeamId
            JOIN [Pick] p on p.UserId = u.Id and p.LeagueId =l.Id
            GROUP BY u.FirstName,
            u.LastName,
            t.School,
            t.Mascot,
            t.Logos,
            l.Name,
            l.Slug";
        return migrationBuilder.Sql(sql);
    }
}