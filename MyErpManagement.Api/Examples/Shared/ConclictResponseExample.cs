using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Examples.Shared
{
    public class ConclictResponseExample : IExamplesProvider<ApiResponseDto>
    {
        public ApiResponseDto GetExamples()
        {
            return new ApiResponseDto(HttpStatusCode.Conflict, ResponseTextConstant.Conflict.DbEntityUniqueField, new Dictionary<string, string>
            {
                ["dtoField*"] = "值 '{Value}' 已存在"
            });
        }
    }
}
