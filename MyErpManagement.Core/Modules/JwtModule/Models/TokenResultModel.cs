using System;
using System.Collections.Generic;
using System.Text;

namespace MyErpManagement.Core.Modules.JwtModule.Models
{
    /// <summary>
    /// JWT 產生結果的承載物件
    /// </summary>
    /// <param name="Token">產出的 JWT 字串</param>
    /// <param name="ExpiresAt">確定的過期時間點（用於資料庫同步）</param>
    public record TokenResultModel(string Token, DateTimeOffset ExpiresAt);
}
