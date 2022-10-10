using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class SeasonCache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeasonCaches",
                columns: table => new
                {
                    Season = table.Column<string>(type: "TEXT", nullable: false),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    LastRefresh = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonCaches", x => new { x.Season, x.Type, x.Week });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonCaches");
        }
    }
}
