using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoModule.Migrations
{
    /// <inheritdoc />
    public partial class AddGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                schema: "Video",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Video",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PreviewVideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_PreviewVideos_PreviewVideoId",
                        column: x => x.PreviewVideoId,
                        principalSchema: "Video",
                        principalTable: "PreviewVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_GroupId",
                schema: "Video",
                table: "Cards",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_PreviewVideoId",
                schema: "Video",
                table: "Groups",
                column: "PreviewVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Groups_GroupId",
                schema: "Video",
                table: "Cards",
                column: "GroupId",
                principalSchema: "Video",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Groups_GroupId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Video");

            migrationBuilder.DropIndex(
                name: "IX_Cards_GroupId",
                schema: "Video",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "Video",
                table: "Cards");
        }
    }
}
