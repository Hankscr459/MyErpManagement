using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class addunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTagRelation_CustomerTag_CusomterTagId",
                table: "CustomerTagRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTagRelation_Customer_CustomerId",
                table: "CustomerTagRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTag",
                table: "CustomerTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "CustomerTag",
                newName: "CustomerTags");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_Code",
                table: "Customers",
                newName: "IX_Customers_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Name",
                table: "ProductCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTags_Name",
                table: "CustomerTags",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTagRelation_CustomerTags_CusomterTagId",
                table: "CustomerTagRelation",
                column: "CusomterTagId",
                principalTable: "CustomerTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTagRelation_Customers_CustomerId",
                table: "CustomerTagRelation",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTagRelation_CustomerTags_CusomterTagId",
                table: "CustomerTagRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTagRelation_Customers_CustomerId",
                table: "CustomerTagRelation");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_Name",
                table: "ProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTags_Name",
                table: "CustomerTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "CustomerTags",
                newName: "CustomerTag");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Code",
                table: "Customer",
                newName: "IX_Customer_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTag",
                table: "CustomerTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTagRelation_CustomerTag_CusomterTagId",
                table: "CustomerTagRelation",
                column: "CusomterTagId",
                principalTable: "CustomerTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTagRelation_Customer_CustomerId",
                table: "CustomerTagRelation",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
