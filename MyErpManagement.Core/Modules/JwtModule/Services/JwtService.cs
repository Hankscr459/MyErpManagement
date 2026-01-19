using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyErpManagement.Core.Modules.JwtModule.Constants;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyErpManagement.Core.Modules.JwtModule.Services
{
    public class JwtService(IConfiguration config) : IJwtService
    {

        public TokenResultModel CreateLogInToken(User user, string jwtRecordId)
        {
            // 統一在此計算時間，建議使用 UtcNow 避免時區問題
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var expiresAt = DateTime.UtcNow.AddDays(Convert.ToInt32(config["JWT_ACCESS_EXPIRATION"]));
            var claims = new List<Claim>
            {
                new Claim(LogInClaimConstant.UserId, user.Id.ToString()),
                new Claim(LogInClaimConstant.Account, user.Account),
                new Claim(LogInClaimConstant.Email, user.Email ?? ""),
                new Claim(LogInClaimConstant.JwtRecordId, jwtRecordId)
            };

            // 使用 HmacSha256 演算法進行簽名。
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // 設定過期時間
                Expires = expiresAt,
                // 用指定的金鑰與演算法，對 JWT 進行數位簽章，確保 Token 的真實性與不可竄改性
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResultModel(tokenHandler.WriteToken(token), expiresAt);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // 1. 設定驗證參數 (建議從 AppSettings 讀取)
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // 2. 執行驗證
                // 此方法會同時驗證簽章與有效性，若驗證失敗會拋出異常
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception)
            {
                // 驗證失敗 (Token 過期、簽章錯誤等)
                return null;
            }
        }
    }
}
