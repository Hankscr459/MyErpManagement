using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using System.Net.Http.Json;

namespace MyErpManagement.IntegrationTests.Tests.Controllers
{
    public class CustomerControllerTests : ApiTestBase
    {
        private readonly ApiWebApplicationFactory _factory;
        public CustomerControllerTests(ApiWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturn204Created()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();

            // 此時的 Client 已經帶有 Bearer Token
            var response = await Client.PostAsJsonAsync(ApiUrlConstant.Customer.CustomerCRUD, new CreateCustomerRequestDto
            {
                Name = CustomerConstant.Name,
                Code = CustomerConstant.Code,
                Notes = CustomerConstant.Notes,
                Address = "123 Test St, Test City",
            });
        }
    }
}
