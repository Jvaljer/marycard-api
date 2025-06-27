using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopifyCustomerId",
                schema: "Order",
                table: "Orders",
                column: "ShopifyCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders",
                column: "ShopifyOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopifyCustomerId",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders");
        }
    }
}
