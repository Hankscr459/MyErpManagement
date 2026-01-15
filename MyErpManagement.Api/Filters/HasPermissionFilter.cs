using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Modules.JwtModule.Constants;
using MyErpManagement.Core.Modules.JwtModule.IRepositories;
using MyErpManagement.Core.Modules.UsersModule.IRepositories;
using System.Net;

namespace MyErpManagement.Api.Filters
{
    public class HasPermissionFilter(string permission, IUserRepository userRepository, IJwtRecordRepository jwtRecordRepository) : IAsyncAuthorizationFilter
    {
        private readonly string _requiredPermission = permission;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtRecordRepository _jwtRecordRepository = jwtRecordRepository;


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // 檢查登入狀態
            if (user.Identity?.IsAuthenticated != true)
            {
                context.SetFailure(HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.InvalidToken);
                return;
            }

            // 取得 UserId
            var userId = user.FindFirst(LogInClaimConstant.UserId)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.SetFailure(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.User);
                return;
            }

            // 移除 "Bearer " 字眼，取得純 Token
            string? token = await context.HttpContext.GetTokenAsync("access_token");
            var jwtRecord = await _jwtRecordRepository.GetJwtRecordByAccessToken(token);
            if (jwtRecord == null)
            {
                context.SetFailure(HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.TokenNotFromServer);
                return;
            }

            var userDetail = await _userRepository.FindUserByIdAsync(userId);
            if (userDetail == null)
            {
                context.SetFailure(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.User);
                return;
            }

            var permissionList = await _userRepository.FindUserPermissionsAsync(userId);


            // 比對權限
            if (userDetail.IsSuperAdmin != true && permissionList?.Contains(_requiredPermission) != true)
            {
                context.SetFailure(HttpStatusCode.Forbidden, ResponseTextConstant.Forbidden.NoPermission);
            }
        }
    }
}
