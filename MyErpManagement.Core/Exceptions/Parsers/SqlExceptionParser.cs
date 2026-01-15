using Microsoft.Data.SqlClient;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Exceptions.IParsers;
using System.Net;
using System.Text.RegularExpressions;

namespace MyErpManagement.Core.Exceptions.Parsers
{
    public class SqlExceptionParser : IExceptionParser
    {
        public ApiResponseDto? Parser(Exception ex)
        {
            if (ex.InnerException is not SqlException sqlEx)
                return null;

            var message = sqlEx.Number switch
            {
                2627 => "資料重複：主鍵衝突。",
                2601 => "資料重複：唯一索引限制。",
                547 => "資料關聯錯誤：此資料正被其他項目引用。",
                _ => $"資料庫執行錯誤 (Code: {sqlEx.Number})"
            };

            // 解析重複值的詳細資訊
            if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
            {
                var match = Regex.Match(sqlEx.Message, @"index '(?<index>.*?)'.*value is \((?<value>.*)\)");
                if (match.Success)
                {
                    var indexName = match.Groups["index"].Value;
                    var fieldName = indexName.Split('_').Last();
                    return new ApiResponseDto(HttpStatusCode.Conflict, message, new Dictionary<string, string[]>
                {
                    { fieldName, [$"值 '{match.Groups["value"].Value}' 已存在"] }
                });
                }
            }

            return new ApiResponseDto(HttpStatusCode.BadRequest, message, sqlEx.Message);
        }
    }
}
