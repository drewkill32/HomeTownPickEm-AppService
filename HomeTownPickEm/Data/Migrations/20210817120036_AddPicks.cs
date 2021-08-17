using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddPicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "League",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeagueSeason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueSeason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueSeason_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLeagueSeason",
                columns: table => new
                {
                    LeagueSeasonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLeagueSeason", x => new { x.LeagueSeasonsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLeagueSeason_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLeagueSeason_LeagueSeason_LeagueSeasonsId",
                        column: x => x.LeagueSeasonsId,
                        principalTable: "LeagueSeason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueSeasonTeam",
                columns: table => new
                {
                    LeagueSeasonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueSeasonTeam", x => new { x.LeagueSeasonsId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_LeagueSeasonTeam_LeagueSeason_LeagueSeasonsId",
                        column: x => x.LeagueSeasonsId,
                        principalTable: "LeagueSeason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueSeasonTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pick",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueSeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Points = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pick", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pick_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pick_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pick_LeagueSeason_LeagueSeasonId",
                        column: x => x.LeagueSeasonId,
                        principalTable: "LeagueSeason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickTeam",
                columns: table => new
                {
                    PicksId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsPickedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickTeam", x => new { x.PicksId, x.TeamsPickedId });
                    table.ForeignKey(
                        name: "FK_PickTeam_Pick_PicksId",
                        column: x => x.PicksId,
                        principalTable: "Pick",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickTeam_Teams_TeamsPickedId",
                        column: x => x.TeamsPickedId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLeagueSeason_MembersId",
                table: "ApplicationUserLeagueSeason",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_League_Name",
                table: "League",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueSeason_LeagueId",
                table: "LeagueSeason",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueSeasonTeam_TeamsId",
                table: "LeagueSeasonTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Pick_GameId",
                table: "Pick",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Pick_LeagueSeasonId",
                table: "Pick",
                column: "LeagueSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Pick_UserId",
                table: "Pick",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PickTeam_TeamsPickedId",
                table: "PickTeam",
                column: "TeamsPickedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserLeagueSeason");

            migrationBuilder.DropTable(
                name: "LeagueSeasonTeam");

            migrationBuilder.DropTable(
                name: "PickTeam");

            migrationBuilder.DropTable(
                name: "Pick");

            migrationBuilder.DropTable(
                name: "LeagueSeason");

            migrationBuilder.DropTable(
                name: "League");
        }
    }
}
