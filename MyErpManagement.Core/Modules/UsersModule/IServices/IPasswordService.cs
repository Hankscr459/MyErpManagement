namespace MyErpManagement.Core.Modules.UsersModule.IServices
{
    public interface IPasswordService
    {
        /// <summary>
        /// 使用指定的鹽值（salt）對密碼進行雜湊。
        /// </summary>
        /// <param name="password">要雜湊的原始密碼。</param>
        /// <param name="salt">用於增強雜湊安全性的鹽值。</param>
        /// <returns>Base64 編碼的雜湊字串。</returns>
        string HashPassword(string password, string salt);
        /// <summary>
        /// 產生一個加密安全的隨機鹽值（Salt）。
        /// </summary>
        /// <returns>Base64 編碼的鹽值字串。</returns>
        string GenerateSalt();
    }
}
