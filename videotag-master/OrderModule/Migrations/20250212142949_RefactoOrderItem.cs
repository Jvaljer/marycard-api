using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class RefactoOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                schema: "Order",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Order",
                table: "OrderProducts");

            migrationBuilder.AddColumn<decimal>(
                name: "ShopifyProductId_ProductId",
                schema: "Order",
                table: "OrderProducts",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ShopifyProductId_VariantId",
                schema: "Order",
                table: "OrderProducts",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopifyProductId_VariantId",
                schema: "Order",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "ShopifyProductId_ProductId",
                schema: "Order",
                table: "OrderProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Order",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "CardId",
                schema: "Order",
                table: "OrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
