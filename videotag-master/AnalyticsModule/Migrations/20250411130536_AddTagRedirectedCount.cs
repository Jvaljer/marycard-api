using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsModule.Migrations
{
    /// <inheritdoc />
    public partial class AddTagRedirectedCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TagRedirected",
                schema: "Analytics",
                table: "CardStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagRedirected",
                schema: "Analytics",
                table: "CardStats");
        }
    }
}
