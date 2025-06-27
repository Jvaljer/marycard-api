using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddMimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mime",
                schema: "Video",
                table: "Videos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mime",
                schema: "Video",
                table: "Videos");
        }
    }
}
