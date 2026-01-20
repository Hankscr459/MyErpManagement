namespace MyErpManagement.Core.Dtos.Auth.Response
{
    public class VerifyEmailResponseDto
    {
        /// <summary>
        /// 訊息
        /// </summary>
        /// <example>驗證碼已發送</example>
        public string Message { get; set; } = default!;

        /// <summary>
        /// 重發驗證碼剩餘幾分鐘
        /// </summary>
        public double ResendIntervalMinutes { get; set; }
    }
}
