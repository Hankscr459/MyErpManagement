using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Shared;
using System.Net;

namespace MyErpManagement.Api.Helpers
{
    public static class ValidationResultHelper
    {
        public static IActionResult HandleInvalidModelState(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key.Replace("$.", ""),
                    kvp => kvp.Value!.Errors.Select(FormatErrorMessage).ToArray()
                );

            return new BadRequestObjectResult(new ApiResponseDto(
                HttpStatusCode.BadRequest,
                ResponseTextConstant.BadRequest.InvalidDto,
                errors));
        }

        private static string FormatErrorMessage(ModelError error)
        {
            // 判斷是否為底層轉型異常
            if (error.Exception != null || error.ErrorMessage.Contains("could not be converted"))
            {
                var msg = error.Exception?.Message ?? error.ErrorMessage;

                string typeFriendlyName = msg switch
                {
                    _ when msg.Contains("System.Double") => "數字 (Double)",
                    _ when msg.Contains("System.Int32") => "整數 (Int32)",
                    _ when msg.Contains("System.DateTime") => "日期時間 (DateTime)",
                    _ when msg.Contains("System.Boolean") => "布林 (Boolean)",
                    _ => "正確的格式"
                };

                return $"欄位格式錯誤，請輸入 {typeFriendlyName}。";
            }

            return error.ErrorMessage;
        }
    }
}
