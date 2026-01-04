using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionOrdersReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class AktualizacjaGwiazdek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "GalleryRatings",
                newName: "UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "GalleryItemId",
                table: "GalleryRatings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GalleryItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_GalleryItemId",
                table: "GalleryRatings",
                column: "GalleryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings",
                columns: new[] { "UserId", "GalleryItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryRatings_GalleryItems_GalleryItemId",
                table: "GalleryRatings",
                column: "GalleryItemId",
                principalTable: "GalleryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryRatings_Users_UserId",
                table: "GalleryRatings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryRatings_GalleryItems_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryRatings_Users_UserId",
                table: "GalleryRatings");

            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GalleryRatings",
                newName: "ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "GalleryItemId",
                table: "GalleryRatings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "GalleryItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
