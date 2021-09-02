using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddCalIdLeague_Part1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Calendar",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Calendar",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalendarMigration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
                    Season = table.Column<string>(type: "TEXT", nullable: true),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonType = table.Column<string>(type: "TEXT", nullable: true),
                    FirstGameStart = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastGameStart = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CutoffDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarMigration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarMigration_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_LeagueId",
                table: "Calendar",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarMigration_LeagueId",
                table: "CalendarMigration",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendar_League_LeagueId",
                table: "Calendar",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            var sql = @"
                INSERT INTO CalendarMigration (Season, Week, CutoffDate, FirstGameStart, LastGameStart,SeasonType, LeagueId)
                SELECT '2021' AS Season , 
                g.Week,
                DATETIME(MIN(g.StartDate),'-5 day','weekday 4', 'start of day','utc') AS CutOffDate,
                Min(g.StartDate) AS FirstGameStart, 
                MAX(g.StartDate) AS LastGameStart,
                'regular' AS SeasonType,
                1 AS LeagueId
                FROM Games g 
                JOIN Pick p ON p.GameId = g.Id 
                GROUP BY g.Week ";
            
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendar_League_LeagueId",
                table: "Calendar");

            migrationBuilder.DropTable(
                name: "CalendarMigration");

            migrationBuilder.DropIndex(
                name: "IX_Calendar_LeagueId",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Calendar");
        }
    }
}
