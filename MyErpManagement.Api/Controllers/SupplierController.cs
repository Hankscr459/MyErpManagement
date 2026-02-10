using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Dtos.Suppliers.Request;
using MyErpManagement.Core.Dtos.Suppliers.Response;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.SupplierModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.DataBase.Helpers;
using System.Net;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Controllers
{
    [Tags("供應商")]
    [SwaggerControllerOrder(3)]
    public class SupplierController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : BaseApiController
    {
        /// <summary>
        /// 新增供應商
        /// </summary>
        /// <param name="createSupplierRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.Supplier.CreateSupplier.Key)]
        public async Task<ActionResult> CreateSupplier([FromBody] CreateSupplierRequestDto createSupplierRequestDto)
        {
            var supplierEntity = mapper.Map<Supplier>(createSupplierRequestDto);
            supplierEntity.Id = new Guid();
            await unitOfWork.BeginTransactionAsync();
            try
            {
                await unitOfWork.SupplierRepository.AddAsync(supplierEntity);
                await unitOfWork.Complete();
                await unitOfWork.SupplierRepository.UpdateSupplierTags(supplierEntity.Id, createSupplierRequestDto.SupplierTagIds);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateSupplier));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看供應商
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpGet("{supplierId}")]
        [HasPermission(PermissionKeysConstant.Supplier.ReadSupplier.Key)]
        public async Task<ActionResult<SupplierResponseDto>> ReadSupplier(Guid supplierId)
        {
            var supplier = await unitOfWork.SupplierRepository.GetQueryable(c => c.Id == supplierId)
                .Include(c => c.SupplierTagRelations)
                .ThenInclude(ctr => ctr.SupplierTag)
                .FirstOrDefaultAsync(c => c.Id == supplierId);
            if (supplier is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Supplier));
            }
            var supplierTags = supplier.SupplierTagRelations
                .Select(ctr => new SupplierTagResponseDto
                {
                    Id = ctr.SupplierTag.Id,
                    Name = ctr.SupplierTag.Name
                }).ToList();
            var supplierResponseDto = mapper.Map<SupplierResponseDto>(supplier);
            supplierResponseDto.SupplierTags = supplierTags;
            return Ok(supplierResponseDto);
        }

        /// <summary>
        /// 供應商清單頁面
        /// </summary>
        /// <param name="supplierListQueryRequestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [HasPermission(PermissionKeysConstant.Supplier.ReadSupplierList.Key)]
        public async Task<ActionResult<SupplierListResponseDto>> ReadSupplierList([FromQuery] SupplierListQueryRequestDto supplierListQueryRequestDto)
        {
            var query = unitOfWork.SupplierRepository.GetSearchQuery(supplierListQueryRequestDto);
            var list = await query.ToPagedListAsync(supplierListQueryRequestDto.Page, supplierListQueryRequestDto.Limit);
            return mapper.Map<SupplierListResponseDto>(list);
        }

        /// <summary>
        /// 修改供應商
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="updateSupplierRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{supplierId}")]
        [HasPermission(PermissionKeysConstant.Supplier.UpdateSupplier.Key)]
        public async Task<IActionResult> UpdateSupplier(Guid supplierId, [FromBody] UpdateSupplierRequestDto updateSupplierRequestDto)
        {
            var supplier = await unitOfWork.SupplierRepository.GetFirstOrDefaultAsync(c => c.Id == supplierId);
            await unitOfWork.BeginTransactionAsync();
            try
            {
                unitOfWork.SupplierRepository.Update(updateSupplierRequestDto.Adapt(supplier));
                if (!await unitOfWork.Complete())
                {
                    return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateSupplier));
                }
                await unitOfWork.CustomerRepository.UpdateCustomerTags(supplier.Id, updateSupplierRequestDto.SupplierTagIds);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateSupplier));
            }

            return NoContent();
        }

        /// <summary>
        /// 移除單筆供應商
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpDelete("{supplierId}")]
        [HasPermission(PermissionKeysConstant.Supplier.DeleteSupplier.Key)]
        public async Task<ActionResult> DeleteSupplier(Guid supplierId)
        {
            var supplier = await unitOfWork.SupplierRepository.GetFirstOrDefaultAsync(c => c.Id == supplierId);
            if (supplier is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Supplier));
            }
            unitOfWork.SupplierRepository.Remove(supplier);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToDeleteSupplier));
            }
            return NoContent();
        }

        /// <summary>
        /// 移除多筆供應商
        /// </summary>
        /// <param name="deleteManySuppliersRequestDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [HasPermission(PermissionKeysConstant.Supplier.DeleteManySuppliers.Key)]
        public async Task<ActionResult> DeleteManySupplier(DeleteManySuppliersRequestDto deleteManySuppliersRequestDto)
        {
            if (deleteManySuppliersRequestDto is null || !deleteManySuppliersRequestDto.Any()) return BadRequest("請提供要刪除的 ID 列表");

            unitOfWork.SupplierRepository.RemoveByIdList(deleteManySuppliersRequestDto);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToDeleteCustomer));
            }
            return NoContent();
        }

        /// <summary>
        /// 新增供應商標籤
        /// </summary>
        /// <param name="createSupplierTagRequestDto"></param>
        /// <returns></returns>
        [HttpPost("supplierTag")]
        [HasPermission(PermissionKeysConstant.SupplierTag.CreateSupplierTag.Key)]
        public async Task<ActionResult> CreateSupplierTag([FromBody] CreateSupplierTagRequestDto createSupplierTagRequestDto)
        {
            var supplierTagEntity = mapper.Map<SupplierTag>(createSupplierTagRequestDto);
            supplierTagEntity.CreatedBy = User.GetUserId();
            await unitOfWork.SupplierTagRepository.AddAsync(supplierTagEntity);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateSupplierTag));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看供應商標籤
        /// </summary>
        /// <param name="supplierTagId"></param>
        /// <returns></returns>
        [HttpGet("supplierTag/{supplierTagId}")]
        [HasPermission(PermissionKeysConstant.SupplierTag.ReadSupplierTag.Key)]
        public async Task<ActionResult<SupplierTag>> GetSupplierTag(Guid supplierTagId)
        {
            var supplierTag = await unitOfWork.SupplierTagRepository.GetFirstOrDefaultAsync(c => c.Id == supplierTagId);
            if (supplierTag is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.SupplierTag));
            }
            return Ok(supplierTag);
        }

        /// <summary>
        /// 查看供應商標籤清單
        /// </summary>
        /// <param name="supplierTagId"></param>
        /// <returns></returns>
        [HttpGet("supplierTags")]
        [HasPermission(PermissionKeysConstant.SupplierTag.ReadSupplierTags.Key)]
        public async Task<ActionResult<IEnumerable<SupplierTag>>> GetSupplierTags(Guid supplierTagId)
        {
            var supplierTags = await unitOfWork.SupplierTagRepository.GetAllAsync(c => true);
            return Ok(supplierTags);
        }

        /// <summary>
        /// 修改供應商標籤
        /// </summary>
        /// <param name="supplierTagId"></param>
        /// <param name="updateSupplierRequestTagDto"></param>
        /// <returns></returns>
        [HttpPut("supplierTag/{supplierTagId}")]
        [HasPermission(PermissionKeysConstant.SupplierTag.UpdateSupplierTag.Key)]
        public async Task<IActionResult> UpdateSupplierTag(Guid supplierTagId, [FromBody] UpdateSupplierRequestTagDto updateSupplierRequestTagDto)
        {
            var supplierTag = await unitOfWork.SupplierTagRepository.GetFirstOrDefaultAsync(c => c.Id == supplierTagId);
            if (supplierTag is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Supplier));
            }

            supplierTag = updateSupplierRequestTagDto.Adapt(supplierTag);
            supplierTag.UpdateAt = DateTime.UtcNow;
            unitOfWork.SupplierTagRepository.Update(supplierTag);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateSupplier));
            }

            return NoContent();
        }
    }
}

