using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseOrderStatusByField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "PurchaseOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CancelBy",
                table: "PurchaseOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompleteBy",
                table: "PurchaseOrders",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CancelBy",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CompleteBy",
                table: "PurchaseOrders");
        }
    }
}
