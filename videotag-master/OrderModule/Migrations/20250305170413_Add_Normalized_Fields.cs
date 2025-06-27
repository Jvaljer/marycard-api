using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class Add_Normalized_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmailNormalized",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmailNormalized",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerFirstNameNormalized",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerLastNameNormalized",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteNormalized",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(max)",
                maxLength: 131071,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailNormalized",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerEmailNormalized",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerFirstNameNormalized",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerLastNameNormalized",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "NoteNormalized",
                schema: "Order",
                table: "Orders");
        }
    }
}
