using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Products.Request;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.DataBase;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace MyErpManagement.IntegrationTests.Tests.Controllers
{
    public class ProductControllerTests : ApiTestBase
    {
        private readonly ApiWebApplicationFactory _factory;
        public ProductControllerTests(ApiWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateProduct_ShouldReturn204Created()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();

            // 此時的 Client 已經帶有 Bearer Token
            var response = await Client.PostAsJsonAsync("/api/product", new CreateProductRequestDto
            {
                Name = ProductConstant.Name,
                Specification = ProductConstant.Specification,
                Code = ProductConstant.Code,
                PurchasePrice = ProductConstant.PurchasePrice,
                SalesPrice = ProductConstant.SalesPrice
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Products.FirstOrDefault(j => j.Code == ProductConstant.Code).Should().NotBeNull();
        }

        [Fact]
        public async Task CreateProduct_DtoError_ShouldReturn400BadRequest()
        {
            await AuthenticateAsync();
            // 此時的 Client 已經帶有 Bearer Token
            var response = await Client.PostAsJsonAsync("/api/product", new
            {
                Specification = ProductConstant.Specification,
                Code = ProductConstant.Code,
                PurchasePrice = ProductConstant.PurchasePrice,
                SalesPrice = ProductConstant.SalesPrice
            });
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>() ?? throw new Exception("新增商品Dto錯誤測試 回傳BadRequest是空直");
            result.Message.Should().Be(ResponseTextConstant.BadRequest.InvalidDto);
        }
    }
}
