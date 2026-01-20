using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.CacheModule.Constants;
using MyErpManagement.Core.Modules.CacheModule.IServices;

namespace MyErpManagement.Core.Modules.CacheModule.Services
{
    public class CachService(IConfiguration config, IDistributedCache cache) : ICachService
    {
        public async Task<int> SaveRegisterCodeAsync(string email, string code)
        {
            var Expiry = Convert.ToInt32(config["Mail_Verify_Register_Code_Expiry"]);
            // 建立過期選項
            var options = new DistributedCacheEntryOptions
            {
                // 設定「絕對過期時間」
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Expiry)
            };
            // 使用介面提供的方法
            await cache.SetStringAsync($"{VerificationCodeConstant.RegisterCodePrefix}{email}", code, options);
            return Expiry;
        }

        public async Task<string?> GetRegistCodeAsync(string email)
        {
            string cacheKey = $"{VerificationCodeConstant.RegisterCodePrefix}{email}";

            // 直接獲取字串
            string? code = await cache.GetStringAsync(cacheKey);

            return code;
        }
    }
}
