using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryModule.Migrations
{
    /// <inheritdoc />
    public partial class AddIllustrationProductAssociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ShopifyProductId_ShopifyVariantId",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ItemCount",
                schema: "Inventory",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ShopifyVariantId",
                schema: "Inventory",
                table: "Products",
                newName: "ShopifyProductId_VariantId");

            migrationBuilder.RenameColumn(
                name: "ShopifyProductId",
                schema: "Inventory",
                table: "Products",
                newName: "ShopifyProductId_ProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Inventory",
                table: "Products",
                newName: "InternalId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "Inventory",
                table: "Products",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "Inventory",
                table: "Products",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "IllustrationProductAssociations",
                schema: "Inventory",
                columns: table => new
                {
                    IllustrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_IllustrationProductAssociations_Illustrations_IllustrationId",
                        column: x => x.IllustrationId,
                        principalSchema: "Inventory",
                        principalTable: "Illustrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IllustrationProductAssociations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Inventory",
                        principalTable: "Products",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IllustrationProductAssociations_IllustrationId",
                schema: "Inventory",
                table: "IllustrationProductAssociations",
                column: "IllustrationId");

            migrationBuilder.CreateIndex(
                name: "IX_IllustrationProductAssociations_ProductId",
                schema: "Inventory",
                table: "IllustrationProductAssociations",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IllustrationProductAssociations",
                schema: "Inventory");

            migrationBuilder.RenameColumn(
                name: "ShopifyProductId_VariantId",
                schema: "Inventory",
                table: "Products",
                newName: "ShopifyVariantId");

            migrationBuilder.RenameColumn(
                name: "ShopifyProductId_ProductId",
                schema: "Inventory",
                table: "Products",
                newName: "ShopifyProductId");

            migrationBuilder.RenameColumn(
                name: "InternalId",
                schema: "Inventory",
                table: "Products",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Inventory",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Inventory",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<long>(
                name: "ItemCount",
                schema: "Inventory",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopifyProductId_ShopifyVariantId",
                schema: "Inventory",
                table: "Products",
                columns: new[] { "ShopifyProductId", "ShopifyVariantId" },
                unique: true);
        }
    }
}
