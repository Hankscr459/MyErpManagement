using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Filters;

namespace MyErpManagement.Api.Attributes
{
    // 改用 TypeFilterAttribute 或 ServiceFilterAttribute 的封裝
    // 繼承 AuthorizeAttribute 會讓框架和 Swagger 把它當作授權標籤
    public class HasPermissionAttribute : TypeFilterAttribute, IAuthorizeData
    {
        public HasPermissionAttribute(string permission) : base(typeof(HasPermissionFilter))
        {
            Arguments = [permission];
        }

        // 這些實作可以留空，繼承 TypeFilterAttribute 主要是為了執行 Filter 邏輯
        public string? Policy { get; set; }
        public string? Roles { get; set; }
        public string? AuthenticationSchemes { get; set; }
    }
}
