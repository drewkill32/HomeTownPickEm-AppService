using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.Data.Migrations
{
    public partial class AddIpAddresUserAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "RefreshToken",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "RefreshToken",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "RefreshToken");
        }
    }
}
