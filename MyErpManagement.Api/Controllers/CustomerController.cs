using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.Core.Dtos.Customers.Response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.DataBase.Helpers;
using System.Net;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Controllers
{
    [Tags("客戶")]
    [SwaggerControllerOrder(2)]
    public class CustomerController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : BaseApiController
    {
        /// <summary>
        /// 新增客戶
        /// </summary>
        /// <param name="createCustomerRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.Customer.CreateCustomer.Key)]
        public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerRequestDto createCustomerRequestDto)
        {
            var customerEntity = mapper.Map<Customer>(createCustomerRequestDto);
            customerEntity.Id = new Guid();
            await unitOfWork.BeginTransactionAsync();
            try
            {
                await unitOfWork.CustomerRepository.AddAsync(customerEntity);
                await unitOfWork.Complete();
                await unitOfWork.CustomerRepository.UpdateCustomerTags(customerEntity.Id, createCustomerRequestDto.CustomerTagIds);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateCustomer));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看客戶
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        [HasPermission(PermissionKeysConstant.Customer.ReadCustomer.Key)]
        public async Task<ActionResult<CustomerResponseDto>> ReadCustomer(Guid customerId)
        {
            var customer = await unitOfWork.CustomerRepository.GetQueryable(c => c.Id == customerId)
                .Include(c => c.CustomerTagRelations)
                .ThenInclude(ctr => ctr.CustomerTag)
                .FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Customer));
            }
            var customerTags = customer.CustomerTagRelations
                .Select(ctr => new CustomerTagResponseDto
                {
                    Id = ctr.CustomerTag.Id,
                    Name = ctr.CustomerTag.Name
                }).ToList();
            var customerResponseDto = mapper.Map<CustomerResponseDto>(customer);
            customerResponseDto.CustomerTags = customerTags;
            return Ok(customerResponseDto);
        }

        /// <summary>
        /// 客戶清單頁面
        /// </summary>
        /// <param name="customerListQueryRequestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(PermissionKeysConstant.Customer.ReadCustomerList.Key)]
        public async Task<ActionResult<CustomerListResponseDto>> ReadCustomerList([FromQuery] CustomerListQueryRequestDto customerListQueryRequestDto)
        {
            var query = unitOfWork.CustomerRepository.GetSearchQuery(customerListQueryRequestDto);
            var list = await query.ToPagedListAsync(customerListQueryRequestDto.Page, customerListQueryRequestDto.Limit);
            return mapper.Map<CustomerListResponseDto>(list);
        }

        /// <summary>
        /// 修改客戶
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="updateCustomerRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{customerId}")]
        [HasPermission(PermissionKeysConstant.Customer.UpdateCustomer.Key)]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, [FromBody] UpdateCustomerRequestDto updateCustomerRequestDto)
        {
            var customer = await unitOfWork.CustomerRepository.GetFirstOrDefaultAsync(c => c.Id == customerId);
            await unitOfWork.BeginTransactionAsync();
            try
            {
                unitOfWork.CustomerRepository.Update(updateCustomerRequestDto.Adapt(customer));
                if (!await unitOfWork.Complete())
                {
                    return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, "ResponseTextConstant.BadRequest.FailToUpdateCustomer"));
                }
                await unitOfWork.CustomerRepository.UpdateCustomerTags(customer.Id, updateCustomerRequestDto.CustomerTagIds);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateCustomer));
            }

            return NoContent();
        }

        /// <summary>
        /// 移除單筆客戶
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        [HasPermission(PermissionKeysConstant.Customer.DeleteCustomer.Key)]
        public async Task<ActionResult> DeleteCustomer(Guid customerId)
        {
            var customer = await unitOfWork.CustomerRepository.GetFirstOrDefaultAsync(c => c.Id == customerId);
            if (customer is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Customer));
            }
            unitOfWork.CustomerRepository.Remove(customer);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToDeleteCustomer));
            }
            return NoContent();
        }

        /// <summary>
        /// 移除多筆客戶
        /// </summary>
        /// <param name="deleteManyCustomersRequestDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [HasPermission(PermissionKeysConstant.Customer.DeleteManyCustomers.Key)]
        public async Task<ActionResult> DeleteManyCustomer(DeleteManyCustomersRequestDto deleteManyCustomersRequestDto)
        {
            if (deleteManyCustomersRequestDto is null || !deleteManyCustomersRequestDto.Any()) return BadRequest("請提供要刪除的 ID 列表");

            unitOfWork.CustomerRepository.RemoveByIdList(deleteManyCustomersRequestDto);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToDeleteCustomer));
            }
            return NoContent();
        }

        /// <summary>
        /// 新增客戶標籤
        /// </summary>
        /// <param name="createCustomerTagRequestDto"></param>
        /// <returns></returns>
        [HttpPost("customerTag")]
        [HasPermission(PermissionKeysConstant.CustomerTag.CreateCustomerTag.Key)]
        public async Task<ActionResult> CreateCustomerTag([FromBody] CreateCustomerTagRequestDto createCustomerTagRequestDto)
        {
            var customerTagEntity = mapper.Map<CustomerTag>(createCustomerTagRequestDto);
            customerTagEntity.CreatedBy = User.GetUserId();
            await unitOfWork.CustomerTagRepository.AddAsync(customerTagEntity);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateCustomerTag));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看客戶標籤
        /// </summary>
        /// <param name="customerTagId"></param>
        /// <returns></returns>
        [HttpGet("customerTag/{customerTagId}")]
        [HasPermission(PermissionKeysConstant.CustomerTag.ReadCustomerTag.Key)]
        public async Task<ActionResult<CustomerTag>> GetCustomerTag(Guid customerTagId)
        {
            var customerTag = await unitOfWork.CustomerTagRepository.GetFirstOrDefaultAsync(c => c.Id == customerTagId);
            if (customerTag is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.CustomerTag));
            }
            return Ok(customerTag);
        }

        /// <summary>
        /// 查看客戶標籤清單
        /// </summary>
        /// <param name="customerTagId"></param>
        /// <returns></returns>
        [HttpGet("customerTags")]
        [HasPermission(PermissionKeysConstant.CustomerTag.ReadCustomerTags.Key)]
        public async Task<ActionResult<IEnumerable<CustomerTag>>> GetCustomerTags(Guid customerTagId)
        {
            var customerTags = await unitOfWork.CustomerTagRepository.GetAllAsync(c => true);
            return Ok(customerTags);
        }

        /// <summary>
        /// 修改客戶標籤
        /// </summary>
        /// <param name="customerTagId"></param>
        /// <param name="updateCustomerRequestTagDto"></param>
        /// <returns></returns>
        [HttpPut("customerTag/{customerTagId}")]
        [HasPermission(PermissionKeysConstant.CustomerTag.UpdateCustomerTag.Key)]
        public async Task<IActionResult> UpdateCustomerTag(Guid customerTagId, [FromBody] UpdateCustomerRequestTagDto updateCustomerRequestTagDto)
        {
            var customerTag = await unitOfWork.CustomerTagRepository.GetFirstOrDefaultAsync(c => c.Id == customerTagId);
            if (customerTag is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Customer));
            }

            customerTag = updateCustomerRequestTagDto.Adapt(customerTag);
            customerTag.UpdateAt = DateTime.UtcNow;
            unitOfWork.CustomerTagRepository.Update(customerTag);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateCustomer));
            }

            return NoContent();
        }
    }
}
