using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Exceptions.IParsers;
using Npgsql;
using System.Net;

namespace MyErpManagement.Core.Exceptions.Parsers
{
    public class SqlExceptionParser : IExceptionParser
    {
        public ApiResponseDto? Parser(Exception ex)
        {
            if (ex.InnerException is not PostgresException pgEx)
                return null;

            var message = pgEx.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation =>
                    "資料重複：唯一限制衝突。",

                PostgresErrorCodes.ForeignKeyViolation =>
                    "資料關聯錯誤：此資料正被其他項目引用。",

                _ =>
                    $"資料庫執行錯誤 (Code: {pgEx.SqlState})"
            };

            // Unique / PK 重複（23505）
            if (pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                var fieldName = pgEx.ConstraintName?.Split('_').Last() ?? "unknown";

                return new ApiResponseDto(
                    HttpStatusCode.Conflict,
                    message,
                    new Dictionary<string, string[]>
                    {
                        { fieldName, [$"值已存在"] }
                    });
            }

            return new ApiResponseDto(
                HttpStatusCode.BadRequest,
                message,
                pgEx.MessageText
            );
        }
    }
}