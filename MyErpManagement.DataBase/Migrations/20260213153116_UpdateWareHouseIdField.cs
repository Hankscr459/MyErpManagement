using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWareHouseIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_WareHouses_WarehouseId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "PurchaseOrders",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_WarehouseId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_WareHouseId");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "InventoryTransactions",
                newName: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_WareHouses_WareHouseId",
                table: "PurchaseOrders",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_WareHouses_WareHouseId",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "PurchaseOrders",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_WareHouseId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "InventoryTransactions",
                newName: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_WareHouses_WarehouseId",
                table: "PurchaseOrders",
                column: "WarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
