using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsModule.Migrations
{
    /// <inheritdoc />
    public partial class RenameCardStatsToPhysicalCardStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CardStats",
                schema: "Analytics",
                table: "CardStats");

            migrationBuilder.RenameTable(
                name: "CardStats",
                schema: "Analytics",
                newName: "PhysicalCardStats",
                newSchema: "Analytics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhysicalCardStats",
                schema: "Analytics",
                table: "PhysicalCardStats",
                column: "CardIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PhysicalCardStats",
                schema: "Analytics",
                table: "PhysicalCardStats");

            migrationBuilder.RenameTable(
                name: "PhysicalCardStats",
                schema: "Analytics",
                newName: "CardStats",
                newSchema: "Analytics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardStats",
                schema: "Analytics",
                table: "CardStats",
                column: "CardIdentifier");
        }
    }
}
