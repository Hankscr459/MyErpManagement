using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Auth
{
    public class BadRequestLoginResponseExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "帳號格式錯誤範例",
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidAccount)
            );

            yield return SwaggerExample.Create(
                "密碼錯誤範例",
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidPassword)
            );

            yield return SwaggerExample.Create(
                "JWT儲存失敗範例",
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToSaveDb)
            );
        }
    }
}
