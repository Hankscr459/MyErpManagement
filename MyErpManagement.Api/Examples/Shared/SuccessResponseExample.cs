using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Shared
{
    public class SuccessResponseExample : IExamplesProvider<ApiResponseDto>
    {
        public ApiResponseDto GetExamples()
        {
            return new ApiResponseDto(HttpStatusCode.OK, "執行成功");
        }
    }
}
