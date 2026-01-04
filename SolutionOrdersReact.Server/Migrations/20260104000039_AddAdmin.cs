using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SolutionOrdersReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_ProductId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryRatings_Clients_ClientId",
                table: "GalleryRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_GalleryRatings_GalleryItems_GalleryItemId",
                table: "GalleryRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_IdCategory",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_UnitOfMeasurements_IdUnitOfMeasurement",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_IdCategory",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_IdUnitOfMeasurement",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_ClientId",
                table: "GalleryRatings");

            migrationBuilder.DropIndex(
                name: "IX_GalleryRatings_GalleryItemId_ClientId",
                table: "GalleryRatings");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ClientId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductId_CreatedAt",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "IdCategory",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "IdClient",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "IdClient",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "IdItem",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "IdItem",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnitOfMeasurements",
                keyColumn: "IdUnitOfMeasurement",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnitOfMeasurements",
                keyColumn: "IdUnitOfMeasurement",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "IdWorker",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "IdWorker",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "IdCategory",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CategoryIdCategory",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GalleryItemId",
                table: "GalleryRatings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Login", "Name", "Password", "Surname" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@demo.pl", "admin", "Admin", "", "System" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryIdCategory",
                table: "Items",
                column: "CategoryIdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items",
                column: "UnitOfMeasurementIdUnitOfMeasurement");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryIdCategory",
                table: "Items",
                column: "CategoryIdCategory",
                principalTable: "Categories",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_UnitOfMeasurements_UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items",
                column: "UnitOfMeasurementIdUnitOfMeasurement",
                principalTable: "UnitOfMeasurements",
                principalColumn: "IdUnitOfMeasurement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryIdCategory",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_UnitOfMeasurements_UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryIdCategory",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CategoryIdCategory",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasurementIdUnitOfMeasurement",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "GalleryItemId",
                table: "GalleryRatings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "IdCategory", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Urządzenia elektroniczne", true, "Elektronika" },
                    { 2, "Produkty spożywcze", true, "Żywność" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "IdClient", "Adress", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "ul. Główna 1, Warszawa", true, "Jan Kowalski", "500-100-200" },
                    { 2, "ul. Kwiatowa 5, Kraków", true, "Anna Nowak", "600-200-300" }
                });

            migrationBuilder.InsertData(
                table: "UnitOfMeasurements",
                columns: new[] { "IdUnitOfMeasurement", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 2, "Kilogramy", true, "kg" },
                    { 3, "Litry", true, "l" }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "IdWorker", "FirstName", "IsActive", "LastName", "Login", "Password" },
                values: new object[,]
                {
                    { 1, "Piotr", true, "Kowalczyk", "pkowalczyk", "haslo123" },
                    { 2, "Maria", true, "Wiśniewska", "mwisnieska", "haslo456" }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "IdItem", "Code", "Description", "FotoUrl", "IdCategory", "IdUnitOfMeasurement", "IsActive", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "LAP001", "Laptop Dell Inspiron 15", null, 1, 1, true, "Laptop Dell", 3500m, 10m },
                    { 2, "MON001", "Monitor 24 cale", null, 1, 1, true, "Monitor Samsung", 800m, 15m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_IdCategory",
                table: "Items",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Items_IdUnitOfMeasurement",
                table: "Items",
                column: "IdUnitOfMeasurement");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_ClientId",
                table: "GalleryRatings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryRatings_GalleryItemId_ClientId",
                table: "GalleryRatings",
                columns: new[] { "GalleryItemId", "ClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClientId",
                table: "Comments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId_CreatedAt",
                table: "Comments",
                columns: new[] { "ProductId", "CreatedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_ProductId",
                table: "Comments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryRatings_Clients_ClientId",
                table: "GalleryRatings",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GalleryRatings_GalleryItems_GalleryItemId",
                table: "GalleryRatings",
                column: "GalleryItemId",
                principalTable: "GalleryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_IdCategory",
                table: "Items",
                column: "IdCategory",
                principalTable: "Categories",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_UnitOfMeasurements_IdUnitOfMeasurement",
                table: "Items",
                column: "IdUnitOfMeasurement",
                principalTable: "UnitOfMeasurements",
                principalColumn: "IdUnitOfMeasurement",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
