using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddApiVideoIdAndLocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiVideoId",
                schema: "Video",
                table: "Videos",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                schema: "Video",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Playable",
                schema: "Video",
                table: "Videos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiVideoId",
                schema: "Video",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Locked",
                schema: "Video",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Playable",
                schema: "Video",
                table: "Videos");
        }
    }
}
