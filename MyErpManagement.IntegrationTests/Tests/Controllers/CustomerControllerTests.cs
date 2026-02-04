using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.DataBase;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using System.Net;
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
            await CustomerTagCheckAsync();

            var response = await Client.PostAsJsonAsync(ApiUrlConstant.Customer.CustomerCRUD, new CreateCustomerRequestDto
            {
                Name = CustomerConstant.Name,
                Code = CustomerConstant.Code,
                Notes = CustomerConstant.Notes,
                Address = CustomerConstant.Address,
                Phone = CustomerConstant.Phone,
                CustomerTagIds = new List<Guid>() { Guid.Parse(CustomerTagConstant.Id) },
            });

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var customer = await db.Customers.Include(c => c.CustomerTagRelations).FirstOrDefaultAsync(j => j.Code == CustomerConstant.Code);
            customer.Should().NotBeNull();
            if (customer != null)
            {
                customer.Name.Should().Be(CustomerConstant.Name);
                customer.Code.Should().Be(CustomerConstant.Code);
                customer.Notes.Should().Be(CustomerConstant.Notes);
                customer.Address.Should().Be(CustomerConstant.Address);
                customer.Phone.Should().Be(CustomerConstant.Phone);
                customer.Balance.Should().Be(0);
                customer.CustomerTagRelations.Should().HaveCount(1);
            }
        }
    }
}
