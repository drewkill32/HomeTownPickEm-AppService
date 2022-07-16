using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonType = table.Column<string>(type: "TEXT", nullable: true),
                    FirstGameStart = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastGameStart = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => new { x.Season, x.Week });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calendar");
        }
    }
}
