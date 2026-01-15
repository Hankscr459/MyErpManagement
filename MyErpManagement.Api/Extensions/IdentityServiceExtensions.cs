using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyErpManagement.Core.Dtos.Shared;
using System.Net;
using System.Text;

namespace MyErpManagement.Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");

                    // 必須設定為 true，否則 Token 不會被持久化到 HttpContext 中
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        // 處理 401 Unauthorized (未登入或 Token 無效)
                        OnChallenge = async context =>
                        {
                            context.HandleResponse(); // 跳過預設處理邏輯
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(new ApiResponseDto(HttpStatusCode.Unauthorized, "驗證失敗，請先登入"));
                        },
                    };

                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        // Header 已有 Bearer Token → 完全不動
                    //        if (!string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                    //        {
                    //            return Task.CompletedTask;
                    //        }

                    //        // Header 沒有 → 才檢查 QueryString
                    //        if (context.Request.Query.TryGetValue("access_token", out var token))
                    //        {
                    //            context.Token = token;
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            return services;
        }
    }
}
