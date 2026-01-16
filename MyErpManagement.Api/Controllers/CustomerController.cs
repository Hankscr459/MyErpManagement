using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Customers.Request;
using MyErpManagement.Core.Dtos.Customers.response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CustomerModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.DataBase.Helpers;
using System.Net;

namespace MyErpManagement.Api.Controllers
{
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
            await unitOfWork.CustomerRepository.AddAsync(customerEntity);
            if (!await unitOfWork.Complete())
            {
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
        public async Task<ActionResult<Customer>> ReadCustomer(Guid customerId)
        {
            var customer = await unitOfWork.CustomerRepository.FindOneByFilterOrUpdate(c => c.Id == customerId);
            if (customer is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Customer));
            }
            return Ok(customer);
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
            var customer = await unitOfWork.CustomerRepository.FindOneByFilterOrUpdate(c => c.Id == customerId);
            if (customer is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Customer));
            }

            unitOfWork.CustomerRepository.Update(updateCustomerRequestDto.Adapt(customer));
            if (!await unitOfWork.Complete())
            {
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
            var customer = await unitOfWork.CustomerRepository.FindOneByFilterOrUpdate(c => c.Id == customerId);
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
        public ActionResult DeleteManyCustomer(DeleteManyCustomersRequestDto deleteManyCustomersRequestDto)
        {
            if (deleteManyCustomersRequestDto is null || !deleteManyCustomersRequestDto.Any()) return BadRequest("請提供要刪除的 ID 列表");

            unitOfWork.CustomerRepository.RemoveByIdList(deleteManyCustomersRequestDto);
            return NoContent();
        }
    }
}
