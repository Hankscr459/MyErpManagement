using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Products.Request;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace MyErpManagement.IntegrationTests.Tests.Controllers
{
    public class ProductControllerTests : ApiTestBase
    {
        public ProductControllerTests(ApiWebApplicationFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// 新增商品 status: 204
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateProduct_ShouldReturn204Created()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();

            // 此時的 Client 已經帶有 Bearer Token
            var response = await Client.PostAsJsonAsync(ApiUrlConstant.Product.ProductCRUD, new CreateProductRequestDto
            {
                Name = ProductConstant.Name,
                Specification = ProductConstant.Specification,
                Code = ProductConstant.Code,
                PurchasePrice = ProductConstant.PurchasePrice,
                SalesPrice = ProductConstant.SalesPrice
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var db = CreateDbContext();
            var product = db.Products.FirstOrDefault(j => j.Code == ProductConstant.Code);
            if (product is null)
            {
                throw new Exception("新增商品後，從資料庫查詢不到該商品");
            }
            product.Name.Should().Be(ProductConstant.Name);
            product.Specification.Should().Be(ProductConstant.Specification);
            product.Code.Should().Be(ProductConstant.Code);
            product.PurchasePrice.Should().Be(ProductConstant.PurchasePrice);
            product.SalesPrice.Should().Be(ProductConstant.SalesPrice);
        }

        [Fact]
        public async Task CreateProduct_DtoError_ShouldReturn400BadRequest()
        {
            await AuthenticateAsync();
            // 此時的 Client 已經帶有 Bearer Token
            var response = await Client.PostAsJsonAsync(ApiUrlConstant.Product.ProductCRUD, new
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
