using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Product
{
    public class CreateProductCategoryFailResponseExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.BadRequest.FailToCreateProductCategory,
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateProductCategory)
            );
        }
    }
}
