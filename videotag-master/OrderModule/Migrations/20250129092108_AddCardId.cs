using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderModule.Migrations
{
    /// <inheritdoc />
    public partial class AddCardId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                schema: "Order",
                table: "OrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                schema: "Order",
                table: "OrderItems");
        }
    }
}
