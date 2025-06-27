using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryModule.Migrations
{
    /// <inheritdoc />
    public partial class AddKeyIllustrationProductAssociation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IllustrationProductAssociations_IllustrationId",
                schema: "Inventory",
                table: "IllustrationProductAssociations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IllustrationProductAssociations",
                schema: "Inventory",
                table: "IllustrationProductAssociations",
                columns: new[] { "IllustrationId", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IllustrationProductAssociations",
                schema: "Inventory",
                table: "IllustrationProductAssociations");

            migrationBuilder.CreateIndex(
                name: "IX_IllustrationProductAssociations_IllustrationId",
                schema: "Inventory",
                table: "IllustrationProductAssociations",
                column: "IllustrationId");
        }
    }
}
