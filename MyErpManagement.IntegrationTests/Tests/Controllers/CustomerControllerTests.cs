using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.Core.Dtos.Customers.response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
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
        public async Task CreateCustomerTag_ShouldReturn204()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();
            var response = await Client.PostAsJsonAsync(ApiUrlConstant.CustomerTag.CustomerTagCRU, new
            CreateCustomerTagRequestDto {
                Name = CustomerTagConstant.CreateName,
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var customer = db.CustomerTags.FirstOrDefault(ct => ct.Name == CustomerTagConstant.CreateName);
            customer.Should().NotBeNull();
        }

        [Fact]
        public async Task ReadCustomerTag_ShouldReturn200()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.CustomerTag.CustomerTagCRU + "/" + CustomerTagConstant.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<CustomerTag>();
            if (result is null)
            {
                throw new Exception("ReadCustomerTag result is null");
            }
            result.Id.Should().Be(Guid.Parse(CustomerTagConstant.Id));
            result.Name.Should().NotBeNull();
        }

        [Fact]
        public async Task ReadCustomerTag_ShouldReturn404()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.CustomerTag.CustomerTagCRU + "/" + Guid.NewGuid());
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
            if (result is null)
            {
                throw new Exception("ReadCustomerTag result is null");
            }
            result.Message.Should().Be(ResponseTextConstant.NotFound.CustomerTag);
        }

        [Fact]
        public async Task ReadCustomerTags_ShouldReturn200()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.CustomerTag.CustomerTags);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerTag>>();
            if (result is null)
            {
                throw new Exception("ReadCustomerTags result is null");
            }
            result.Should().Contain(c => c.Id == Guid.Parse(CustomerTagConstant.Id));
        }

        [Fact]
        public async Task UpdateCustomerTag_ShouldReturn204()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            
            var response = await Client.PutAsJsonAsync(ApiUrlConstant.CustomerTag.CustomerTagCRU + "/" + CustomerTagConstant.Id, new
            UpdateCustomerRequestTagDto
            {
                Name = CustomerTagConstant.UpdateName,
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var updateCustomerTag = await db.CustomerTags.FirstOrDefaultAsync(ct => ct.Id == Guid.Parse(CustomerTagConstant.Id));
            if (updateCustomerTag is null)
            {
                throw new Exception("UpdateCustomerTag not found");
            }

            updateCustomerTag.Name.Should().Be(CustomerTagConstant.UpdateName);
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturn204Created()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();
            await CustomerTagCheckAsync();

            var response = await Client.PostAsJsonAsync(ApiUrlConstant.Customer.CustomerCRUD, new CreateCustomerRequestDto
            {
                Name = CustomerConstant.CreateName,
                Code = CustomerConstant.CreateCode,
                Notes = CustomerConstant.CreateNotes,
                Address = CustomerConstant.CreateAddress,
                Phone = CustomerConstant.CreatePhone,
                CustomerTagIds = new List<Guid>() { Guid.Parse(CustomerTagConstant.Id) },
            });

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var customer = await db.Customers.Include(c => c.CustomerTagRelations).FirstOrDefaultAsync(j => j.Code == CustomerConstant.CreateCode);
            customer.Should().NotBeNull();
            if (customer != null)
            {
                customer.Name.Should().Be(CustomerConstant.CreateName);
                customer.Code.Should().Be(CustomerConstant.CreateCode);
                customer.Notes.Should().Be(CustomerConstant.CreateNotes);
                customer.Address.Should().Be(CustomerConstant.CreateAddress);
                customer.Phone.Should().Be(CustomerConstant.CreatePhone);
                customer.Balance.Should().Be(0);
                customer.CustomerTagRelations.Should().HaveCount(1);
                customer.CustomerTagRelations.FirstOrDefault(c => c.CusomterTagId == Guid.Parse(CustomerTagConstant.Id)).Should().NotBeNull();
            }
        }

        [Fact]
        public async Task ReadCustomer_ShouldReturn200()
        {
            await AuthenticateAsync();
            await CustomerCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.Customer.CustomerCRUD + "/" + CustomerConstant.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Customer>();
            if (result is null)
            {
                throw new Exception("ReadCustomer result is null");
            }
            result.Id.Should().Be(CustomerConstant.Id);
            result.Code.Should().Be(CustomerConstant.Code);
            result.Address.Should().Be(CustomerConstant.Address);
            result.Phone.Should().Be(CustomerConstant.Phone);
            result.Notes.Should().Be(CustomerConstant.Notes);
            result.Balance.Should().Be(0);
            result.CustomerTagRelations.Should().HaveCount(0);
        }

        [Fact]
        public async Task ReadCustomer_ShouldReturn404()
        {
            await AuthenticateAsync();
            await CustomerCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.Customer.CustomerCRUD + "/" + Guid.NewGuid());
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
            if (result is null)
            {
                throw new Exception("ReadCustomer result is null");
            }
            result.Message.Should().Be(ResponseTextConstant.NotFound.Customer);
        }

        [Fact]
        public async Task ReadCustomers_ShouldReturn200()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            var response = await Client.GetAsync(ApiUrlConstant.Customer.CustomerCRUD);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<CustomerListResponseDto>();
            if (result is null)
            {
                throw new Exception("ReadCustomers result is null");
            }
            result.Docs.Should().Contain(c => c.Id == Guid.Parse(CustomerConstant.Id));
            result.Page.Should().Be(1);
            result.Limit.Should().Be(10);
        }

        [Fact]
        public async Task UpdateCustomers_ShouldReturn200()
        {
            await AuthenticateAsync();
            await CustomerTagCheckAsync();
            var response = await Client.PutAsJsonAsync(ApiUrlConstant.Customer.CustomerCRUD + "/" + CustomerConstant.Id, new
            UpdateCustomerRequestDto
            {
                Name = CustomerTagConstant.UpdateName,
                Code = CustomerConstant.UpdateCode,
                Notes = CustomerConstant.UpdateNotes,
                Address = CustomerConstant.UpdateAddress,
                Phone = CustomerConstant.UpdatePhone,
                CustomerTagIds = new List<Guid>() { Guid.Parse(CustomerTagConstant.Id) },
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var customer = await db.Customers.Include(c => c.CustomerTagRelations).FirstOrDefaultAsync(c => c.Id == Guid.Parse(CustomerConstant.Id));
            if (customer is null)
            {
                throw new Exception("UpdateCustomer not found");
            }
            customer.Id.Should().Be(Guid.Parse(CustomerConstant.Id));
            customer.Name.Should().Be(CustomerTagConstant.UpdateName);
            customer.Code.Should().Be(CustomerConstant.UpdateCode);
            customer.Notes.Should().Be(CustomerConstant.UpdateNotes);
            customer.Address.Should().Be(CustomerConstant.UpdateAddress);
            customer.Phone.Should().Be(CustomerConstant.UpdatePhone);
            customer.CustomerTagRelations.Should().Contain(c => c.CusomterTagId == Guid.Parse(CustomerTagConstant.Id));
        }
    }
}
