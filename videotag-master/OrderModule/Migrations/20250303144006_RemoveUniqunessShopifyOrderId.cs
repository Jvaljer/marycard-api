using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqunessShopifyOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders",
                column: "ShopifyOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShopifyOrderId",
                schema: "Order",
                table: "Orders",
                column: "ShopifyOrderId",
                unique: true);
        }
    }
}
