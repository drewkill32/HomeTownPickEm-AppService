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
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Season = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLeague",
                columns: table => new
                {
                    LeaguesId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLeague", x => new { x.LeaguesId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLeague_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLeague_League_LeaguesId",
                        column: x => x.LeaguesId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueTeam",
                columns: table => new
                {
                    LeaguesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueTeam", x => new { x.LeaguesId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_LeagueTeam_League_LeaguesId",
                        column: x => x.LeaguesId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueTeam_Teams_TeamsId",
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
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_Pick_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
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
                name: "IX_ApplicationUserLeague_MembersId",
                table: "ApplicationUserLeague",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_League_Name_Season",
                table: "League",
                columns: new[] { "Name", "Season" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeam_TeamsId",
                table: "LeagueTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_Pick_GameId",
                table: "Pick",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Pick_LeagueId",
                table: "Pick",
                column: "LeagueId");

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
                name: "ApplicationUserLeague");

            migrationBuilder.DropTable(
                name: "LeagueTeam");

            migrationBuilder.DropTable(
                name: "PickTeam");

            migrationBuilder.DropTable(
                name: "Pick");

            migrationBuilder.DropTable(
                name: "League");
        }
    }
}
