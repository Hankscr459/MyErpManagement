using Microsoft.EntityFrameworkCore;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Exceptions.IParsers;
using System.Net;

namespace MyErpManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parsers"></param>
        /// parsers 會寫 IEnumerable<IExceptionParser>原因
        /// 當你定義為 IEnumerable 時，DI 容器會把所有註冊為 IExceptionParser 的類別全部收集起來湊成一個集合丟給 Middleware
        /// SqlExceptionParser（處理資料庫）services.AddSingleton<IExceptionParser, SqlExceptionParser>();
        /// ValidationExceptionParser（處理 FluentValidation）services.AddSingleton<IExceptionParser, ValidationExceptionParser>();
        /// IdentityExceptionParser（處理權限認證）services.AddSingleton<IExceptionParser, IdentityExceptionParser>();
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IEnumerable<IExceptionParser> parsers)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                // 處裡整理過的錯誤(目前只有DB錯誤有整理)
                ApiResponseDto? response = null;
                foreach (var parser in parsers)
                {
                    if (ex is not DbUpdateException dbEx) break;
                    response = parser.Parser(ex);
                    if (response != null) break;
                }

                // 如果沒有 DB錯誤，回傳預設 500
                response ??= new ApiResponseDto(HttpStatusCode.InternalServerError, "Internal Server Error",
                    _env.IsDevelopment() ? ex.StackTrace : ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
