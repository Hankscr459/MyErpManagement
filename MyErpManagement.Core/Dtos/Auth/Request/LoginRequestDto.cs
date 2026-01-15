using System.ComponentModel.DataAnnotations;

namespace MyErpManagement.Core.Dtos.Auth.Request
{
    /// <summary>
    /// 登入請求物件
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        /// <example>Admin01</example>
        [Required]
        [MinLength(4)]
        public string Account { get; set; } = "";
        /// <summary>
        /// 密碼
        /// </summary>
        /// <example>123456</example>
        [Required]
        [MinLength(4)]
        public string Password { get; set; } = "";
    }
}
