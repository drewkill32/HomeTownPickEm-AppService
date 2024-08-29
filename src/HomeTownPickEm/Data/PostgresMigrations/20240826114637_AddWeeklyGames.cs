using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.PostgresMigrations
{
    /// <inheritdoc />
    public partial class AddWeeklyGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    SeasonId = table.Column<int>(type: "integer", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    Week = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklyGames_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyGamePicks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    WeeklyGameId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    TotalPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyGamePicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyGamePicks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeeklyGamePicks_WeeklyGames_WeeklyGameId",
                        column: x => x.WeeklyGameId,
                        principalTable: "WeeklyGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyGamePicks_UserId",
                table: "WeeklyGamePicks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyGamePicks_WeeklyGameId",
                table: "WeeklyGamePicks",
                column: "WeeklyGameId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyGames_GameId",
                table: "WeeklyGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyGames_SeasonId",
                table: "WeeklyGames",
                column: "SeasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeeklyGamePicks");

            migrationBuilder.DropTable(
                name: "WeeklyGames");
        }
    }
}
