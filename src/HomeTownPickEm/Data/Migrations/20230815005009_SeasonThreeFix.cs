using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class SeasonThreeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
delete from Pick where SeasonId ='3';
delete from SeasonTeam where SeasonsId = '3';
delete from SeasonCaches where Season ='3';
delete from ApplicationUserSeason where SeasonsId ='3';
delete from Season where Id ='3';
update SQLITE_SEQUENCE
set seq = 0
where name ='Season';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
