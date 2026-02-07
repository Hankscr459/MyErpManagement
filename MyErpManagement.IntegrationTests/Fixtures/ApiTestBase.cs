using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Dtos.Auth.Response;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.DataBase;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.SeedData;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyErpManagement.IntegrationTests.Fixtures
{
    public abstract class ApiTestBase : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly HttpClient Client;
        protected readonly ApiWebApplicationFactory Factory;

        protected ApiTestBase(ApiWebApplicationFactory factory)
        {
            Factory = factory;
            Client = factory.CreateClient();
        }

        protected ApplicationDbContext CreateDbContext()
        { 
            var scope = Factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        // 封裝登入邏輯，供子類別呼叫
        protected async Task AuthenticateAsync()
        {
            await TestUserSeed.Initialize(Factory);

            var response = await Client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Account = UserConstant.Account,
                Password = UserConstant.Password
            });
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (string.IsNullOrEmpty(result?.Token))
            {
                throw new Exception("無法取得 Token，登入失敗。");
            }

            // 將 Token 加入 HttpClient 的 Header 中
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result.Token);
        }

        protected async Task CustomerTagCheckAsync()
        {
            var db = this.CreateDbContext();
            if (await db.CustomerTags.AnyAsync(u => u.Id == Guid.Parse(CustomerTagConstant.Id)))
            {
                return;
            }

            var userId = Guid.Parse(UserConstant.Id);
            db.CustomerTags.Add(new CustomerTag
            {
                Id = Guid.Parse(CustomerTagConstant.Id),
                Name = CustomerTagConstant.Name,
                CreatedBy = userId,
            });
            await db.SaveChangesAsync();
        }

        protected async Task CustomerCheckAsync()
        {
            var db = this.CreateDbContext();
            if (await db.Customers.AnyAsync(u => u.Id == Guid.Parse(CustomerConstant.Id)))
            {
                return;
            }
            var userId = Guid.Parse(UserConstant.Id);
            db.Customers.Add(new Customer
            {
                Id = Guid.Parse(CustomerConstant.Id),
                Name = CustomerConstant.Name,
                Phone = CustomerConstant.Phone,
                Address = CustomerConstant.Address,
                Code = CustomerConstant.Code,
                Notes = CustomerConstant.Notes,
            });
            await db.SaveChangesAsync();
        }
    }
}
