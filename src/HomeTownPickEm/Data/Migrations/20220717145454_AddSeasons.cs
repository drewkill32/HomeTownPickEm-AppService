using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddSeasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [Leaderboard];");
            
            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Pick",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Season_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserSeason",
                columns: table => new
                {
                    MembersId = table.Column<string>(type: "TEXT", nullable: false),
                    SeasonsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSeason", x => new { x.MembersId, x.SeasonsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSeason_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSeason_Season_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeasonTeam",
                columns: table => new
                {
                    SeasonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTeam", x => new { x.SeasonsId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_SeasonTeam_Season_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pick_SeasonId",
                table: "Pick",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSeason_SeasonsId",
                table: "ApplicationUserSeason",
                column: "SeasonsId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_LeagueId",
                table: "Season",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTeam_TeamsId",
                table: "SeasonTeam",
                column: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick");

            migrationBuilder.DropTable(
                name: "ApplicationUserSeason");

            migrationBuilder.DropTable(
                name: "SeasonTeam");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropIndex(
                name: "IX_Pick_SeasonId",
                table: "Pick");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Pick");
        }
    }
}
