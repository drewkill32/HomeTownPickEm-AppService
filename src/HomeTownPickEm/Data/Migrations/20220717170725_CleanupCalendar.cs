using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class CleanupCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendar_League_LeagueId",
                table: "Calendar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar");

            migrationBuilder.DropIndex(
                name: "IX_Calendar_LeagueId",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "CutoffDate",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Calendar");

            migrationBuilder.AlterColumn<string>(
                name: "SeasonType",
                table: "Calendar",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Season",
                table: "Calendar",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar",
                columns: new[] { "Week", "Season", "SeasonType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar");

            migrationBuilder.AlterColumn<string>(
                name: "SeasonType",
                table: "Calendar",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Season",
                table: "Calendar",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Calendar",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CutoffDate",
                table: "Calendar",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Calendar",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_LeagueId",
                table: "Calendar",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendar_League_LeagueId",
                table: "Calendar",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
