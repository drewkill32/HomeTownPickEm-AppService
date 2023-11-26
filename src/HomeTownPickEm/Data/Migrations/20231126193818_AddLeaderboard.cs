using HomeTownPickEm.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeTownPickEm.src.HomeTownPickEm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaderboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddLeaderboardView();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropLeaderboardView();
        }
    }
}
