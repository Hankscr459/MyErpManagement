using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Validators.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace MyErpManagement.Api.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var declaringType = context.MethodInfo.DeclaringType;
            if (declaringType is null) return;

            // 處理 400 Bad Request (RequestBody 檢查) & 檢查是否有 Request Body (通常是 POST, PUT)
            HandleBadRequest(operation, context);

            // 處理 401, 403, 404 (授權與權限檢查)
            HandleAuthorization(operation, context, declaringType);
        }

        private void HandleBadRequest(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasRequestBody = context.ApiDescription.ParameterDescriptions
                .Any(p => p.Source.Id == "Body");
            if (!hasRequestBody) return;

            var examples = new Dictionary<string, OpenApiExample>
            {
                [ResponseTextConstant.BadRequest.InvalidDto] = CreateExample(
                    (int)HttpStatusCode.BadRequest,
                    ResponseTextConstant.BadRequest.InvalidDto,
                    ResponseTextConstant.BadRequest.InvalidDto,
                    new OpenApiObject
                    {
                        ["dtoField*"] = new OpenApiArray
                        {
                            new OpenApiString(VaildatorConstant.Required),
                            new OpenApiString("長度至少需要 {MinLength} 位")
                        }
                    })
            };

            AddResponseWithExamples(operation, context, "400", "Bad Request", examples);
        }


        private void HandleAuthorization(OpenApiOperation operation, OperationFilterContext context, Type declaringType)
        {
            var hasAuthorize = declaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            var hasPermission = declaringType.GetCustomAttributes(true).OfType<HasPermissionAttribute>().Any() ||
                                context.MethodInfo.GetCustomAttributes(true).OfType<HasPermissionAttribute>().Any();

            if (!hasAuthorize && !hasPermission) return;

            // 設定 Security Requirement
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = Array.Empty<string>()
            });

            // 401 Examples
            var authExamples = new Dictionary<string, OpenApiExample>
            {
                ["InvalidToken"] = CreateExample((int)HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.InvalidToken, "無效的權杖(Token)"),
                ["TokenNotFromServer"] = CreateExample((int)HttpStatusCode.Unauthorized, ResponseTextConstant.UnAuthorized.TokenNotFromServer, "來源不明的權杖(Token)")
            };
            AddResponseWithExamples(operation, context, "401", "未經授權 (Unauthorized)", authExamples);

            // 403 Examples
            var forbiddenExamples = new Dictionary<string, OpenApiExample>
            {
                ["NoPermission"] = CreateExample((int)HttpStatusCode.Forbidden, ResponseTextConstant.Forbidden.NoPermission, "此使用者的所有角色(Role)沒有該權限操作")
            };
            AddResponseWithExamples(operation, context, "403", "權限不足 (Forbidden)", forbiddenExamples);

            // 404 Examples
            var notFoundExamples = new Dictionary<string, OpenApiExample>
            {
                ["UserNotFound"] = CreateExample((int)HttpStatusCode.NotFound, ResponseTextConstant.NotFound.User, "權杖(Token)解密的使用者Id查不到此使用者")
            };
            AddResponseWithExamples(operation, context, "404", "查無資料 (NotFound)", notFoundExamples);
        }


        private void AddResponseWithExamples(OpenApiOperation operation, OperationFilterContext context, string statusCode, string description, IDictionary<string, OpenApiExample> examples)
        {
            // 如果 Response 已存在則獲取，否則新增
            if (!operation.Responses.TryGetValue(statusCode, out var response))
            {
                response = new OpenApiResponse
                {
                    Description = description,
                    Content = GetErrorSchema(context)
                };
                operation.Responses.TryAdd(statusCode, response);
            }

            if (response.Content.TryGetValue("application/json", out var mediaType))
            {
                mediaType.Examples ??= new Dictionary<string, OpenApiExample>();
                foreach (var example in examples)
                {
                    // 使用 TryAdd 避免重複 Key 報錯
                    mediaType.Examples.TryAdd(example.Key, example.Value);
                }
            }
        }

        private OpenApiExample CreateExample(int statusCode, string message, string description, IOpenApiAny details = null)
        {
            return new OpenApiExample
            {
                Summary = description,
                Value = new OpenApiObject
                {
                    ["statusCode"] = new OpenApiInteger(statusCode),
                    ["message"] = new OpenApiString(message),
                    ["details"] = details ?? new OpenApiNull()
                }
            };
        }

        private IDictionary<string, OpenApiMediaType> GetErrorSchema(OperationFilterContext context)
        {
            return new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponseDto), context.SchemaRepository)
                }
            };
        }
    }
}
