using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalyticsModule.Migrations
{
    /// <inheritdoc />
    public partial class AddCardStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardStats",
                schema: "Analytics",
                columns: table => new
                {
                    CardIdentifier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhysicalCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardVisited = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardStats", x => x.CardIdentifier);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardStats",
                schema: "Analytics");
        }
    }
}
