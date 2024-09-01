using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.PostgresMigrations;

/// <inheritdoc />
public partial class AddWeeklyLeaderboardView : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
drop view ""WeeklyLeaderboard"";

create or replace view ""WeeklyLeaderboard""
            (""UserId"", ""UserFirstName"", ""UserLastName"", ""TeamId"", ""TeamSchool"", ""TeamMascot"", ""TeamLogos"", ""LeagueId"",
             ""LeagueName"", ""LeagueSlug"", ""Week"", ""SeasonId"", ""Year"", ""TotalPoints"")
as
SELECT u.""Id""                       AS ""UserId"",
       u.""FirstName""                AS ""UserFirstName"",
       u.""LastName""                 AS ""UserLastName"",
       t.""Id""                       AS ""TeamId"",
       t.""School""                   AS ""TeamSchool"",
       t.""Mascot""                   AS ""TeamMascot"",
       t.""Logos""                    AS ""TeamLogos"",
       l.""Id""                       AS ""LeagueId"",
       l.""Name""                     AS ""LeagueName"",
       l.""Slug""                     AS ""LeagueSlug"",
       g.""Week""                     AS ""Week"",
       s.""Id""                       AS ""SeasonId"",
       s.""Year""                     AS ""Year"",
       sum(COALESCE(p.""Points"", 0)) AS ""TotalPoints""
FROM ""Season"" s
         JOIN ""League"" l ON s.""LeagueId"" = l.""Id""
         JOIN ""ApplicationUserSeason"" aus ON aus.""SeasonsId"" = s.""Id""
         JOIN ""AspNetUsers"" u ON u.""Id"" = aus.""MembersId""
         JOIN ""Teams"" t ON t.""Id"" = u.""TeamId""
         LEFT JOIN ""Pick"" p ON p.""UserId"" = u.""Id"" AND p.""SeasonId"" = s.""Id""
         LEFT JOIN ""Games"" g ON g.""Id"" = p.""GameId""
GROUP BY s.""Id"", s.""Year"", g.""Week"", u.""Id"", u.""FirstName"", u.""LastName"", t.""Id"", t.""School"", t.""Mascot"", t.""Logos"", l.""Id"", l.""Name"", l.""Slug"";
");
    }


    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
drop view ""WeeklyLeaderboard"";
create or replace view ""WeeklyLeaderboard""
            (""UserId"", ""UserFirstName"", ""UserLastName"", ""TeamId"", ""TeamSchool"", ""TeamMascot"", ""TeamLogos"", ""LeagueId"",
             ""LeagueName"", ""LeagueSlug"", ""Week"", ""Year"", ""TotalPoints"")
as
SELECT u.""Id""                       AS ""UserId"",
       u.""FirstName""                AS ""UserFirstName"",
       u.""LastName""                 AS ""UserLastName"",
       t.""Id""                       AS ""TeamId"",
       t.""School""                   AS ""TeamSchool"",
       t.""Mascot""                   AS ""TeamMascot"",
       t.""Logos""                    AS ""TeamLogos"",
       l.""Id""                       AS ""LeagueId"",
       l.""Name""                     AS ""LeagueName"",
       l.""Slug""                     AS ""LeagueSlug"",
       g.""Week""                    AS ""Week"",
       s.""Year"",
        sum(COALESCE(p.""Points"", 0)) AS ""TotalPoints""
FROM ""Season"" s
         JOIN ""League"" l ON s.""LeagueId"" = l.""Id""
         JOIN ""ApplicationUserSeason"" aus ON aus.""SeasonsId"" = s.""Id""
         JOIN ""AspNetUsers"" u ON u.""Id"" = aus.""MembersId""
         JOIN ""Teams"" t ON t.""Id"" = u.""TeamId""
         LEFT JOIN ""Pick"" p ON p.""UserId"" = u.""Id"" AND p.""SeasonId"" = s.""Id""
         LEFT JOIN ""Games"" g ON g.""Id"" = p.""GameId""
GROUP BY s.""Year"",g.""Week"", u.""Id"", u.""FirstName"", u.""LastName"", t.""Id"", t.""School"", t.""Mascot"", t.""Logos"", l.""Id"", l.""Name"", l.""Slug"";");
    }
}