using MyErpManagement.Core.Modules.JwtModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using System.Security.Claims;

namespace MyErpManagement.Core.Modules.JwtModule.IServices
{
    // 這個介面定義了生成 JWT 存取權杖所需的功能。
    public interface IJwtService
    {
        /// <summary>
        /// 依據使用者資訊產生 JWT 存取權杖（Access Token）
        /// </summary>
        /// <param name="user">
        /// 使用者資料，將包含於 JWT Claim,
        /// 包含：
        ///    — 使用者唯一識別碼(user_id)
        ///    — 使用者帳號(account)
        /// </param>
        /// <returns>
        /// 回傳已簽章的 JWT 字串
        /// </returns>
        public TokenResultModel CreateLogInToken(User user, string jwtRecordId);
        public ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}
