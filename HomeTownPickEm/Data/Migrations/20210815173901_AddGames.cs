using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Season = table.Column<string>(type: "TEXT", nullable: true),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonType = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    StartTimeTbd = table.Column<bool>(type: "INTEGER", nullable: false),
                    HomeId = table.Column<int>(type: "INTEGER", nullable: false),
                    HomePoints = table.Column<int>(type: "INTEGER", nullable: true),
                    AwayId = table.Column<int>(type: "INTEGER", nullable: false),
                    AwayPoints = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Teams_AwayId",
                        column: x => x.AwayId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Teams_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_AwayId",
                table: "Games",
                column: "AwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HomeId",
                table: "Games",
                column: "HomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
