using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.Core.Dtos.Customers.Response;
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

        /// <summary>
        /// 新增客戶標籤，並確認資料庫的資料是否正確 status: 204
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateCustomerTag_ShouldReturn204()
        {
            // 呼叫基底類別的方法，自動幫 Client 加上 Header
            await AuthenticateAsync();
            var response = await Client.PostAsJsonAsync(ApiUrlConstant.CustomerTag.CustomerTagCRU, new
            CreateCustomerTagRequestDto
            {
                Name = CustomerTagConstant.CreateName,
            });
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var customer = db.CustomerTags.FirstOrDefault(ct => ct.Name == CustomerTagConstant.CreateName);
            customer.Should().NotBeNull();
        }

        /// <summary>
        /// 查看客戶標籤資料 status: 200
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// 查無客戶標籤 status: 404
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// 查看客戶標籤清單 status: 200
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            // 確認回傳的資料包含剛剛新增的客戶標籤
            result.Should().Contain(c => c.Id == Guid.Parse(CustomerTagConstant.Id));
        }

        /// <summary>
        /// 修改客戶標籤 status: 204
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            // 確認名稱欄位更新
            updateCustomerTag.Name.Should().Be(CustomerTagConstant.UpdateName);
        }

        /// <summary>
        /// 新增客戶，並確認資料庫的資料是否正確 status: 204
        /// </summary>
        /// <returns></returns>
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
                // 確認每個欄位都有存進去
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

        /// <summary>
        /// 查看客戶資料 status: 200
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            // 確認每個欄位都有拿到預期的值
            result.Id.Should().Be(CustomerConstant.Id);
            result.Code.Should().Be(CustomerConstant.Code);
            result.Address.Should().Be(CustomerConstant.Address);
            result.Phone.Should().Be(CustomerConstant.Phone);
            result.Notes.Should().Be(CustomerConstant.Notes);
            result.Balance.Should().Be(0);
            result.CustomerTagRelations.Should().HaveCount(0);
        }

        /// <summary>
        /// 查無客戶測試 status: 404
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// 客戶分頁清單 sttatus: 200
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            // 確認回傳的分頁資料
            result.Docs.Should().Contain(c => c.Id == Guid.Parse(CustomerConstant.Id));
            result.Page.Should().Be(1);
            result.Limit.Should().Be(10);
        }

        /// <summary>
        /// 修改客戶資料，並確認資料庫的資料是否正確 status: 200
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            // 確認每個欄位是否更新
            customer.Id.Should().Be(Guid.Parse(CustomerConstant.Id));
            customer.Name.Should().Be(CustomerTagConstant.UpdateName);
            customer.Code.Should().Be(CustomerConstant.UpdateCode);
            customer.Notes.Should().Be(CustomerConstant.UpdateNotes);
            customer.Address.Should().Be(CustomerConstant.UpdateAddress);
            customer.Phone.Should().Be(CustomerConstant.UpdatePhone);
            customer.CustomerTagRelations.Should().Contain(c => c.CusomterTagId == Guid.Parse(CustomerTagConstant.Id));
        }

        /// <summary>
        /// 移除客戶單筆 status: 204
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteCustomer_ShouldReturn204()
        {
            await AuthenticateAsync();
            await CustomerCheckAsync();
            var response = await Client.DeleteAsync(ApiUrlConstant.Customer.CustomerCRUD + "/" + CustomerConstant.Id);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var db = CreateDbContext();
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == Guid.Parse(CustomerConstant.Id));
            customer.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCustomers_shouldReturn204()
        {
            await AuthenticateAsync();
            var db = CreateDbContext();

            db.Customers.AddRange(new List<Customer>()
            {
                new Customer
                {
                    Id = Guid.Parse(CustomerConstant.DeleteId),
                    Name = CustomerConstant.DeleteName,
                    Code = CustomerConstant.DeleteCode,
                    Notes = CustomerConstant.DeleteNotes,
                    Address = CustomerConstant.DeleteAddress,
                    Phone = CustomerConstant.DeletePhone,
                },
                new Customer
                {
                    Id = Guid.Parse(CustomerConstant.Delete2Id),
                    Name = CustomerConstant.Delete2Name,
                    Code = CustomerConstant.Delete2Code,
                    Notes = CustomerConstant.Delete2Notes,
                    Address = CustomerConstant.Delete2Address,
                    Phone = CustomerConstant.Delete2Phone,
                }
            });
            await db.SaveChangesAsync();
            DeleteManyCustomersRequestDto list = new DeleteManyCustomersRequestDto
            {
                Guid.Parse(CustomerConstant.DeleteId),
                Guid.Parse(CustomerConstant.Delete2Id)
            };
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                ApiUrlConstant.Customer.CustomerCRUD
            )
            {
                Content = JsonContent.Create(list)
            };
            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var deleteCustomer = await db.Customers.FirstOrDefaultAsync(c => c.Id == Guid.Parse(CustomerConstant.DeleteId));
            deleteCustomer.Should().BeNull();
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == Guid.Parse(CustomerConstant.Delete2Id));
            customer.Should().BeNull();
        }
    }
}
