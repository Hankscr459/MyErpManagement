using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updateInventoryFieldTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_WareHouses_WarehouseId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Inventories",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_WarehouseId",
                table: "Inventories",
                newName: "IX_Inventories_WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductId_WarehouseId",
                table: "Inventories",
                newName: "IX_Inventories_ProductId_WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_WareHouses_WareHouseId",
                table: "Inventories",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_WareHouses_WareHouseId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "Inventories",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_WareHouseId",
                table: "Inventories",
                newName: "IX_Inventories_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductId_WareHouseId",
                table: "Inventories",
                newName: "IX_Inventories_ProductId_WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_WareHouses_WarehouseId",
                table: "Inventories",
                column: "WarehouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
