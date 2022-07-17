using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class MigrateSeason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [Season] (LeagueId, Year)
                SELECT Id, Season
                FROM League");
            
            migrationBuilder.Sql(@"INSERT INTO [ApplicationUserSeason] (MembersId, SeasonsId)
                SELECT appl.MembersId,  s.Id AS SeasonId
                FROM Season s
                JOIN League l on l.Id = s.LeagueId AND l.Season = s.'Year'
                JOIN ApplicationUserLeague  appl on appl.LeaguesId = l.Id ");
            
            migrationBuilder.Sql(@"INSERT INTO [SeasonTeam] (TeamsId, SeasonsId)
                SELECT appl.TeamsId,  s.Id AS SeasonId
                FROM Season s
                JOIN League l on l.Id = s.LeagueId AND l.Season = s.'Year'
                JOIN LeagueTeam  appl on appl.LeaguesId = l.Id ");
            
            migrationBuilder.Sql(@"UPDATE Pick SET SeasonId = s.Id
                FROM Pick p
                JOIN League l on l.Id = p.LeagueId 
                JOIN Season s on l.Id = s.LeagueId AND l.Season = s.'Year'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
