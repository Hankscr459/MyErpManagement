using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Shared
{
    public class ProductNotFoundResponseExample : IExamplesProvider<ApiResponseDto>
    {
        public ApiResponseDto GetExamples()
        {
            return new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Product);
        }
    }
}
