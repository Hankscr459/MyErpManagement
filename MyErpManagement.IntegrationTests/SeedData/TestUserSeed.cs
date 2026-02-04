using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.DataBase;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using System.Security.Cryptography;
using System.Text;

namespace MyErpManagement.IntegrationTests.SeedData
{
    public static class TestUserSeed
    {
        public static async Task Initialize(ApiWebApplicationFactory factory)
        {
            var userId = new Guid(UserConstant.Id);
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (await db.Users.AnyAsync(u => u.Id == userId))
            {
                return;
            }
            // 定義測試用的原始密碼
            string rawPassword = UserConstant.Password;
            // 固定一個 Salt，方便測試環境使用（也可以隨機產生）
            string salt = "TestUserSpecificSalt123==";

            // 實作與你的 PasswordService 相同的 SHA256 加密邏輯
            string hash = CalculateHash(rawPassword, salt);
            try
            {
                db.Users.Add(new User
                {
                    Id = new Guid(UserConstant.Id),
                    Account = UserConstant.Account,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    IsSuperAdmin = true,
                    CreatedAt = DateTime.UtcNow
                });
                await db.SaveChangesAsync();
            }
            catch (ArgumentException)
            {
                // 如果在 SaveChanges 時剛好被另一個 Thread 塞進去了，就忽略它
            }
        }
        private static string CalculateHash(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
