using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolutionOrdersReact.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersMinimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_IdItem",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_IdOrder",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_IdClient",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Workers_IdWorker",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_IdItem",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_IdOrder",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DataOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdOrderItem",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IdItem",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IdOrder",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "IdWorker",
                table: "Orders",
                newName: "WorkerIdWorker");

            migrationBuilder.RenameColumn(
                name: "IdClient",
                table: "Orders",
                newName: "ClientIdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdWorker",
                table: "Orders",
                newName: "IX_Orders_WorkerIdWorker");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdClient",
                table: "Orders",
                newName: "IX_Orders_ClientIdClient");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "OrderItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemIdItem",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderItems",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "OrderItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SourceItemId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemIdItem",
                table: "OrderItems",
                column: "ItemIdItem");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_ItemIdItem",
                table: "OrderItems",
                column: "ItemIdItem",
                principalTable: "Items",
                principalColumn: "IdItem");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientIdClient",
                table: "Orders",
                column: "ClientIdClient",
                principalTable: "Clients",
                principalColumn: "IdClient");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Workers_WorkerIdWorker",
                table: "Orders",
                column: "WorkerIdWorker",
                principalTable: "Workers",
                principalColumn: "IdWorker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Items_ItemIdItem",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientIdClient",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Workers_WorkerIdWorker",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ItemIdItem",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ItemIdItem",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SourceItemId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "WorkerIdWorker",
                table: "Orders",
                newName: "IdWorker");

            migrationBuilder.RenameColumn(
                name: "ClientIdClient",
                table: "Orders",
                newName: "IdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_WorkerIdWorker",
                table: "Orders",
                newName: "IX_Orders_IdWorker");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientIdClient",
                table: "Orders",
                newName: "IX_Orders_IdClient");

            migrationBuilder.AddColumn<int>(
                name: "IdOrder",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataOrder",
                table: "Orders",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Orders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdOrderItem",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdItem",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdOrder",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "IdOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "IdOrderItem");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_IdItem",
                table: "OrderItems",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_IdOrder",
                table: "OrderItems",
                column: "IdOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Items_IdItem",
                table: "OrderItems",
                column: "IdItem",
                principalTable: "Items",
                principalColumn: "IdItem",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_IdOrder",
                table: "OrderItems",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "IdOrder",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_IdClient",
                table: "Orders",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Workers_IdWorker",
                table: "Orders",
                column: "IdWorker",
                principalTable: "Workers",
                principalColumn: "IdWorker",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
