using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OrlandoServices.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemAndModelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Service_ServiceId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderFieldValue_Order_OrderId",
                table: "OrderFieldValue");

            migrationBuilder.DropIndex(
                name: "IX_Order_ServiceId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderFieldValue",
                newName: "OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderFieldValue_OrderId",
                table: "OrderFieldValue",
                newName: "IX_OrderFieldValue_OrderItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "User",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ServiceField",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "ServiceField",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "ServiceField",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Service",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Service",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FieldNameAtOrderTime",
                table: "OrderFieldValue",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FieldTypeAtOrderTime",
                table: "OrderFieldValue",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Order",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "Donation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestName",
                table: "Donation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ServiceId",
                table: "OrderItem",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFieldValue_OrderItem_OrderItemId",
                table: "OrderFieldValue",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderFieldValue_OrderItem_OrderItemId",
                table: "OrderFieldValue");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ServiceField");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "ServiceField");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ServiceField");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "FieldNameAtOrderTime",
                table: "OrderFieldValue");

            migrationBuilder.DropColumn(
                name: "FieldTypeAtOrderTime",
                table: "OrderFieldValue");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "Donation");

            migrationBuilder.DropColumn(
                name: "GuestName",
                table: "Donation");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "OrderFieldValue",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderFieldValue_OrderItemId",
                table: "OrderFieldValue",
                newName: "IX_OrderFieldValue_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ServiceId",
                table: "Order",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Service_ServiceId",
                table: "Order",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderFieldValue_Order_OrderId",
                table: "OrderFieldValue",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
