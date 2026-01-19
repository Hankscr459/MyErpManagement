using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Product
{
    public class UpdateProductCategoryFailResponseExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.BadRequest.FailToUpdateProductCategory,
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateProductCategory)
            );
        }
    }
}
