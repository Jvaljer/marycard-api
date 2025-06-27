using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderItemProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "Order",
                table: "OrderItems",
                newName: "PhysicalCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhysicalCardId",
                schema: "Order",
                table: "OrderItems",
                newName: "ItemId");
        }
    }
}
