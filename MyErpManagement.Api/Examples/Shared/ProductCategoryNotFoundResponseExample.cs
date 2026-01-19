using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Shared
{
    public class ProductCategoryNotFoundResponseExample : IExamplesProvider<ApiResponseDto>
    {
        public ApiResponseDto GetExamples()
        {
            return new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.ProductCategory);
        }
    }
}
