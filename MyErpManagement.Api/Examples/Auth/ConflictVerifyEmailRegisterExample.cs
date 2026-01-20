using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Auth
{
    public class ConflictVerifyEmailRegisterExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.Conflict.DbEntityUniqueField,
                new ApiResponseDto(
                    HttpStatusCode.Conflict,
                    ResponseTextConstant.Conflict.EmailAlreadyInUse)
            );
        }
    }
}
