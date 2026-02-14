using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.PurchaseOrder.Request;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.Enums;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.OrderNoModule.Enums;
using MyErpManagement.Core.Modules.OrderNoModule.IServices;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Entities;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Enums;
using MyErpManagement.Core.Modules.PurchaseOrderModule.IServices;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using System.Net;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Controllers
{
    [Tags("採購單")]
    [SwaggerControllerOrder(5)]
    public class PurchaseOrderController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOrderNoService orderNoService,
        IPurchaseOrderService purchaseOrderService,
        IInventoryService inventoryService,
        IInventoryTransactionService inventoryTransactionService
    ) : BaseApiController
    {
        /// <summary>
        /// 新增採購單
        /// </summary>
        /// <param name="createPurchaseOrderRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.CreatePurchaseOrder.Key)]
        public async Task<ActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderRequestDto createPurchaseOrderRequestDto)
        {
            var purchaseOrder = mapper.Map<PurchaseOrder>(createPurchaseOrderRequestDto);
            purchaseOrder.CreatedBy = User.GetUserId();
            purchaseOrder.Status = PurchaseOrderStatusEnum.Draft;
            purchaseOrder.TotalPrice = purchaseOrderService.CountTotalPrice(mapper.Map<IList<PurchaseOrderCountTotalPriceModel>>(createPurchaseOrderRequestDto.Lines));
            await unitOfWork.BeginTransactionAsync();
            try
            {
                purchaseOrder.OrderNo = await orderNoService.GeneratePrivateOrderNo(OrderTypeEnum.PO, createPurchaseOrderRequestDto.OrderDate);
                await unitOfWork.PurchaseOrderRepository.AddAsync(purchaseOrder);
                await unitOfWork.Complete();
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreatePurchaseOrder));
            }
            
            return NoContent();
        }

        /// <summary>
        /// 核准採購單
        /// </summary>
        /// <returns></returns>
        [HttpPost("{purchaseOrderId}/approve")]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.ApprovePurchaseOrder.Key)]
        public async Task<ActionResult> ApprovePurchaseOrder(Guid purchaseOrderId)
        {
            var purchaseOrder = await unitOfWork.PurchaseOrderRepository.GetFirstOrDefaultAsync(po => po.Id == purchaseOrderId, "Lines");
            if (purchaseOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.PurchaseOrder));
            }
            await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var line in purchaseOrder.Lines)
                {
                    await inventoryService.AddInventoryByCreatePurchaseOrder(new InventoryModel
                    {
                        ProductId = line.ProductId,
                        WareHouseId = purchaseOrder.WareHouseId,
                        Quantity = line.Quantity,
                        Price = line.Price,
                        CreatedBy = User.GetUserId(),
                    });
                    await inventoryTransactionService.AddInventoryTransaction(new AddInventoryTransactionModel
                    {
                        ProductId = line.ProductId,
                        WareHouseId = purchaseOrder.WareHouseId,
                        QuantityChange = line.Quantity,
                        UnitCost = line.Price,
                        SourceType = InventorySourceTypeEnum.PurchaseOrder,
                        SourceId = purchaseOrder.Id,
                        CreatedBy = User.GetUserId(),
                    });
                }
                purchaseOrder.Status = PurchaseOrderStatusEnum.Approved;
                unitOfWork.PurchaseOrderRepository.Update(purchaseOrder);
                await unitOfWork.Complete();
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToApprovePurchaseOrder));
            }
            return NoContent();
        }

        /// <summary>
        /// 取消採購單
        /// </summary>
        /// <returns></returns>
        [HttpPost("{purchaseOrderId}/cancel")]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.CancelPurchaseOrder.Key)]
        public async Task<ActionResult> CancelPurchaseOrder(Guid purchaseOrderId)
        {
            var purchaseOrder = await unitOfWork.PurchaseOrderRepository.GetFirstOrDefaultAsync(po => po.Id == purchaseOrderId, "Lines");
            if (purchaseOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.PurchaseOrder));
            }
            if (purchaseOrder.Status != PurchaseOrderStatusEnum.Approved)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCancelPurchaseOrder));
            }
            await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var line in purchaseOrder.Lines)
                {
                    /*** 取消採購單時，將相關的庫存恢復 ***/
                    var inventoryModelArg = new InventoryModel
                    {
                        ProductId = line.ProductId,
                        WareHouseId = purchaseOrder.WareHouseId,
                        Quantity = line.Quantity,
                        Price = line.Price,
                        CreatedBy = User.GetUserId(),
                    };
                    if (!await inventoryService.RestoreInventoryByCancelPurchaseOrder(inventoryModelArg))
                    {
                        return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Inventory));
                    }

                    /*** 取消採購單時，將相關的庫存交易紀錄移除 ***/
                    var inventoryTransaction = await unitOfWork.InventoryTransactionRepository.GetFirstOrDefaultAsync(
                        it => it.SourceType == InventorySourceTypeEnum.PurchaseOrder && it.SourceId == purchaseOrder.Id);
                    if (inventoryTransaction is not null)
                    {
                        unitOfWork.InventoryTransactionRepository.Remove(inventoryTransaction);
                    }

                }
                purchaseOrder.Status = PurchaseOrderStatusEnum.Cancelled;
                unitOfWork.PurchaseOrderRepository.Update(purchaseOrder);
                await unitOfWork.Complete();
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToApprovePurchaseOrder, ex.Message));
            }
            return NoContent();
        }

        /// <summary>
        /// 完成採購單
        /// </summary>
        /// <returns></returns>
        [HttpPost("{purchaseOrderId}/complete")]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.CompletePurchaseOrder.Key)]
        public async Task<ActionResult> CompletePurchaseOrder(Guid purchaseOrderId)
        {
            var purchaseOrder = await unitOfWork.PurchaseOrderRepository.GetFirstOrDefaultAsync(po => po.Id == purchaseOrderId, "Lines");
            if (purchaseOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.PurchaseOrder));
            }
            if (purchaseOrder.Status == PurchaseOrderStatusEnum.Draft)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCompletePurchaseOrder));
            }
            purchaseOrder.Status = PurchaseOrderStatusEnum.Completed;
            unitOfWork.PurchaseOrderRepository.Update(purchaseOrder);
            await unitOfWork.Complete();
            return NoContent();
        }

        /// <summary>
        /// 修改採購單
        /// </summary>
        /// <returns></returns>
        [HttpPut("{purchaseOrderId}")]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.ApprovePurchaseOrder.Key)]
        public async Task<ActionResult> UpdatePurchaseOrder(Guid purchaseOrderId,[FromBody]UpdatePurchaseOrderRequestDto updatePurchaseOrderRequestDto)
        {
            var purchaseOrder = await unitOfWork.PurchaseOrderRepository.GetFirstOrDefaultAsync(po => po.Id == purchaseOrderId);
            if (purchaseOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.PurchaseOrder));
            }
            if (purchaseOrder.Status != PurchaseOrderStatusEnum.Draft)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdatePurchaseOrderForNotDrift));
            }
            var updateOrderDate = updatePurchaseOrderRequestDto.OrderDate.ToString("yyyyMMdd");
            string datePart = purchaseOrder.OrderNo.Split('-')[1];
            if (updateOrderDate != datePart)
            {
                purchaseOrder.OrderNo = await orderNoService.GeneratePrivateOrderNo(OrderTypeEnum.PO, updatePurchaseOrderRequestDto.OrderDate);
            }
            unitOfWork.PurchaseOrderRepository.Update(updatePurchaseOrderRequestDto.Adapt(purchaseOrder));
            return NoContent();
        }
    }
}
