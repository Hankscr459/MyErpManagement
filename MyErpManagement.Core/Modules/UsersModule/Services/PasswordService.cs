using MyErpManagement.Core.Modules.UsersModule.IServices;
using System.Security.Cryptography;
using System.Text;

namespace MyErpManagement.Core.Modules.UsersModule.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password, string salt)
        {
            // 使用 SHA256 演算法建立一個新的雜湊計算器。
            // 'using var' 確保在區塊結束時會自動釋放資源 (Dispose)。
            using var sha256 = SHA256.Create();

            // 1. 將密碼和鹽值組合起來 (password + salt)。
            // 2. 使用 UTF8 編碼將組合後的字串轉換成位元組陣列。
            var bytes = Encoding.UTF8.GetBytes(password + salt);

            // 計算組合後位元組陣列的 SHA256 雜湊值。
            var hash = sha256.ComputeHash(bytes);

            // 將雜湊後的位元組陣列轉換成 Base64 字串並回傳。
            // Base64 格式方便儲存和傳輸。
            return Convert.ToBase64String(hash);
        }

        public string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
