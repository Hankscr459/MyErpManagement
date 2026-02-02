using System;
using System.Collections.Generic;
using System.Text;

namespace MyErpManagement.Core.Modules.JwtModule.Entities
{
    public class JwtRecord
    {
        public Guid Id { get; set; } // 這也可以直接當作 JWT 裡的 jti claim

        public Guid UserId { get; set; }

        // 建議存 Token 的 Hash 值，或是只存 Refresh Token
        public string TokenValue { get; set; } = default!;

        // 強制登出的關鍵：紀錄該 Token 是否已被廢棄
        public bool IsRevoked { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }

        // 追蹤來源 (推薦加入)
        public string? RemoteIpAddress { get; set; }
        public string? UserAgent { get; set; } // 辨識是哪台電腦或瀏覽器
    }
}
