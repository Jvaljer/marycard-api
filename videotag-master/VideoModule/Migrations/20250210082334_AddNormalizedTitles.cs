using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedTitles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedTitle",
                schema: "Video",
                table: "Videos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedTitle",
                schema: "Video",
                table: "PreviewVideos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedTitle",
                schema: "Video",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "NormalizedTitle",
                schema: "Video",
                table: "PreviewVideos");
        }
    }
}
