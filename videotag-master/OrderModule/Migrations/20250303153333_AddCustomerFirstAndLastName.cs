using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerFirstAndLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerFirstName",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerLastName",
                schema: "Order",
                table: "Orders",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerFirstName",
                schema: "Order",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerLastName",
                schema: "Order",
                table: "Orders");
        }
    }
}
