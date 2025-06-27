using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviewVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Video_VideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Video",
                schema: "Video",
                table: "Video");

            migrationBuilder.RenameTable(
                name: "Video",
                schema: "Video",
                newName: "Videos",
                newSchema: "Video");

            migrationBuilder.AddColumn<Guid>(
                name: "PreviewVideoId",
                schema: "Video",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Videos",
                schema: "Video",
                table: "Videos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PreviewVideos",
                schema: "Video",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApiVideoId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Playable = table.Column<bool>(type: "bit", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviewVideos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PreviewVideoId",
                schema: "Video",
                table: "Cards",
                column: "PreviewVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_PreviewVideos_PreviewVideoId",
                schema: "Video",
                table: "Cards",
                column: "PreviewVideoId",
                principalSchema: "Video",
                principalTable: "PreviewVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Videos_VideoId",
                schema: "Video",
                table: "Cards",
                column: "VideoId",
                principalSchema: "Video",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_PreviewVideos_PreviewVideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Videos_VideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "PreviewVideos",
                schema: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Cards_PreviewVideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Videos",
                schema: "Video",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "PreviewVideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Videos",
                schema: "Video",
                newName: "Video",
                newSchema: "Video");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Video",
                schema: "Video",
                table: "Video",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Video_VideoId",
                schema: "Video",
                table: "Cards",
                column: "VideoId",
                principalSchema: "Video",
                principalTable: "Video",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
