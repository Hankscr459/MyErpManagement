using Microsoft.AspNetCore.Mvc.Filters;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Modules.JwtModule.Constants;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;
using System.Net;

namespace MyErpManagement.Api.Filters
{
    public class HasQueryTokenFilter(
        IJwtService jwtService,
        IUserRepository userRepository,
        IJwtRecordRepository jwtRecordRepository
    ) : IAsyncAuthorizationFilter
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtRecordRepository _jwtRecordRepository = jwtRecordRepository;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 1. 嘗試從 Query 取得 Token
            if (!context.HttpContext.Request.Query.TryGetValue("access_token", out var tokenValue) ||
                string.IsNullOrWhiteSpace(tokenValue))
            {
                context.SetFailure(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.RequiredQueryAccessToken);
                return;
            }
            var token = tokenValue.ToString();
            try
            {
                // 2. 驗證並解碼 Token (建議 IJwtService 內部實作 TokenValidationParameters 驗證)
                var principal = _jwtService.GetPrincipalFromToken(token);
                if (principal == null)
                {
                    context.SetFailure(HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.InvalidToken);
                    return;
                }
                // 3. 安全地提取 UserId
                var userIdClaim = principal.FindFirst(LogInClaimConstant.UserId)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    context.SetFailure(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.User);
                    return;
                }

                var jwtRecord = _jwtRecordRepository.GetJwtRecordByAccessToken(token);
                if (jwtRecord == null)
                {
                    context.SetFailure(HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.InvalidToken);
                    return;
                }

                var user = await _userRepository.FindUserByIdAsync(userIdClaim);
                if (user == null)
                {
                    context.SetFailure(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.User);
                    return;
                }
                // 5. 將驗證後的使用者資訊掛載至 HttpContext，供後續 Request 週期使用
                context.HttpContext.Items["CurrentUser"] = user;
                context.HttpContext.User = principal;
            }
            catch (Exception)
            {
                context.SetFailure(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }
    }
}
