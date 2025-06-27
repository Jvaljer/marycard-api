using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class RenameVideoToCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                schema: "Video",
                table: "Videos");

            migrationBuilder.RenameTable(
                name: "Videos",
                schema: "Video",
                newName: "Cards",
                newSchema: "Video");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_Identifier",
                schema: "Video",
                table: "Cards",
                newName: "IX_Cards_Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                schema: "Video",
                table: "Cards",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                schema: "Video",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Cards",
                schema: "Video",
                newName: "Videos",
                newSchema: "Video");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_Identifier",
                schema: "Video",
                table: "Videos",
                newName: "IX_Videos_Identifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                schema: "Video",
                table: "Videos",
                column: "Id");
        }
    }
}
