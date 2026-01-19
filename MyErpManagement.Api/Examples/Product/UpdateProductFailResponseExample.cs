using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Product
{
    public class UpdateProductFailResponseExample : IMultipleExamplesProvider<ApiResponseDto>
    {
        public IEnumerable<SwaggerExample<ApiResponseDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                ResponseTextConstant.BadRequest.FailToUpdateProduct,
                new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateProduct)
            );
        }
    }
}
