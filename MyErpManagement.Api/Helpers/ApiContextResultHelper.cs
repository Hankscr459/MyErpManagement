using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyErpManagement.Core.Dtos.Shared;
using System.Net;

namespace MyErpManagement.Api.Helpers
{
    public static class ApiContextResultHelper
    {
        public static void SetFailure(this AuthorizationFilterContext context, HttpStatusCode statusCode, string message, object? details = null)
        {
            // 這裡套用你現有的 ApiExceptionDto
            var response = new ApiResponseDto(statusCode, message, details);

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}
