using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class ChangingPicks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickTeam");

            migrationBuilder.AddColumn<int>(
                name: "SelectedTeamId",
                table: "Pick",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pick_SelectedTeamId",
                table: "Pick",
                column: "SelectedTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_Teams_SelectedTeamId",
                table: "Pick",
                column: "SelectedTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pick_Teams_SelectedTeamId",
                table: "Pick");

            migrationBuilder.DropIndex(
                name: "IX_Pick_SelectedTeamId",
                table: "Pick");

            migrationBuilder.DropColumn(
                name: "SelectedTeamId",
                table: "Pick");

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
                name: "IX_PickTeam_TeamsPickedId",
                table: "PickTeam",
                column: "TeamsPickedId");
        }
    }
}
