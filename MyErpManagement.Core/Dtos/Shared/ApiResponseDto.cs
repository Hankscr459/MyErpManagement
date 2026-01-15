using System.Net;

namespace MyErpManagement.Core.Dtos.Shared
{
    public class ApiResponseDto(HttpStatusCode statusCode, string message, object? details = null)
    {
        /// <summary>
        /// HttpStatus
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = message;
        /// <summary>
        /// 明細(Object)
        /// </summary>
        public object? Details { get; set; } = details;
    }
}
