using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionOrdersReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GalleryRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TargetType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    TargetId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings",
                columns: new[] { "UserId", "GalleryItemId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_TargetType_TargetId_CreatedAt",
                table: "ActivityLogs",
                columns: new[] { "TargetType", "TargetId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId_CreatedAt",
                table: "ActivityLogs",
                columns: new[] { "UserId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GalleryRatings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_UserId_GalleryItemId",
                table: "GalleryRatings",
                columns: new[] { "UserId", "GalleryItemId" },
                unique: true);
        }
    }
}
