using MyErpManagement.Core.Dtos.Auth.Request;
using Swashbuckle.AspNetCore.Filters;

namespace MyErpManagement.Api.Examples.Auth
{
    public class RequestBodyLoginExample : IMultipleExamplesProvider<LoginRequestDto>
    {
        public IEnumerable<SwaggerExample<LoginRequestDto>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "測試",
                new LoginRequestDto { Account = "admin", Password = "password" }
            );
            yield return SwaggerExample.Create(
                "無效的帳號",
                new LoginRequestDto { Account = "admin01", Password = "password123" }
            );

            yield return SwaggerExample.Create(
                "無效的密碼",
                new LoginRequestDto { Account = "admin", Password = "password123" }
            );
        }
    }
}
