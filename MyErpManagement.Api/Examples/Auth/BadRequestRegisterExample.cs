using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Auth
{
    public class BadRequestRegisterExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.BadRequest.FailToSendEmailCode,
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidEmailCode)
            );

            yield return SwaggerExample.Create(
                ResponseTextConstant.BadRequest.FailToSendEmailCode,
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateUser)
            );
        }
    }
}
