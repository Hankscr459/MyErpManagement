using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class CreateTransferOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TransferNo = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FromWareHouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWareHouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferOrders_WareHouses_FromWareHouseId",
                        column: x => x.FromWareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrders_WareHouses_ToWareHouseId",
                        column: x => x.ToWareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TransferOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferOrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferOrderLines_TransferOrders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLines_ProductId",
                table: "TransferOrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLines_TransferOrderId",
                table: "TransferOrderLines",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_FromWareHouseId",
                table: "TransferOrders",
                column: "FromWareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_ToWareHouseId",
                table: "TransferOrders",
                column: "ToWareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_TransferNo",
                table: "TransferOrders",
                column: "TransferNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferOrderLines");

            migrationBuilder.DropTable(
                name: "TransferOrders");
        }
    }
}
