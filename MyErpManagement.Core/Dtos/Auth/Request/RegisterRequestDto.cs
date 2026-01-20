namespace MyErpManagement.Core.Dtos.Auth.Request
{
    public class RegisterRequestDto
    {
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 使用者電子郵件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 註冊驗證碼
        /// </summary>
        public string VerificationCode { get; set; }
    }
}
