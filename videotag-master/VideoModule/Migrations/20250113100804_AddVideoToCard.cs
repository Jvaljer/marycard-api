using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                schema: "Video",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Video",
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
                    table.PrimaryKey("PK_Video", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_VideoId",
                schema: "Video",
                table: "Cards",
                column: "VideoId",
                unique: true,
                filter: "[VideoId] IS NOT NULL");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Video_VideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Video",
                schema: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Cards_VideoId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "VideoId",
                schema: "Video",
                table: "Cards");
        }
    }
}
