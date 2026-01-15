using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyErpManagement.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class PendingChangesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b743b802-1234-4567-8901-abcdef123456"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "qBVVlvYaQKHxqrW4uuMD4srNufExODr2mPBQRO3VPog=", "9VfZybPBbBQD0xon+8J9lw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b743b802-1234-4567-8901-abcdef123456"),
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "+WVt4u71R5CnmHlF2b/6X2Tbk1bqebfFHauy4=", "1nUlJs1bhK1l+qGEgmp/aA==" });
        }
    }
}
