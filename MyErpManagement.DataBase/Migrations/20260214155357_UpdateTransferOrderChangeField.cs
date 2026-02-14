using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransferOrderChangeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransferNo",
                table: "TransferOrders",
                newName: "OrderNo");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrders_TransferNo",
                table: "TransferOrders",
                newName: "IX_TransferOrders_OrderNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderNo",
                table: "TransferOrders",
                newName: "TransferNo");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrders_OrderNo",
                table: "TransferOrders",
                newName: "IX_TransferOrders_TransferNo");
        }
    }
}
