using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class AddContextToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerPhone",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
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
                name: "CustomerPhone",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "Order",
                table: "Orders");
        }
    }
}
