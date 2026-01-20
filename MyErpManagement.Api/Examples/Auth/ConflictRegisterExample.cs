using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Auth
{
    public class ConflictRegisterExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.Conflict.DbEntityUniqueField,
                new ApiResponseDto(
                    HttpStatusCode.Conflict,
                    ResponseTextConstant.Conflict.DbEntityUniqueField, new Dictionary<string, string>
                    {
                        ["Email"] = "值 '{Value}' 已存在"
                    }
                )
            );
        }
    }
}
