using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoFrame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mime",
                schema: "Video",
                table: "Videos",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "FileId",
                schema: "Video",
                table: "Videos",
                newName: "VideoTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                schema: "Video",
                table: "Videos",
                newName: "Mime");

            migrationBuilder.RenameColumn(
                name: "VideoTitle",
                schema: "Video",
                table: "Videos",
                newName: "FileId");
        }
    }
}
