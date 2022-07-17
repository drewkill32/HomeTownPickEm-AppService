using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class RemoveUnusedLeagueFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pick_League_LeagueId",
                table: "Pick");

            migrationBuilder.DropForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick");

            migrationBuilder.DropTable(
                name: "ApplicationUserLeague");

            migrationBuilder.DropTable(
                name: "LeagueTeam");

            migrationBuilder.DropIndex(
                name: "IX_Pick_LeagueId",
                table: "Pick");

            migrationBuilder.DropIndex(
                name: "IX_League_Name_Season",
                table: "League");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Pick");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "League");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "Pick",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "Pick",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Pick",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "League",
                type: "TEXT",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Pick_LeagueId",
                table: "Pick",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_League_Name_Season",
                table: "League",
                columns: new[] { "Name", "Season" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLeague_MembersId",
                table: "ApplicationUserLeague",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeam_TeamsId",
                table: "LeagueTeam",
                column: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_League_LeagueId",
                table: "Pick",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_Season_SeasonId",
                table: "Pick",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id");
        }
    }
}
