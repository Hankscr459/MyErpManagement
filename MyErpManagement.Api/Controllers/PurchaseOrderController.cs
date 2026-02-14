using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.PurchaseOrder.Request;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.Entities;
using MyErpManagement.Core.Modules.InventoryModule.Enums;
using MyErpManagement.Core.Modules.InventoryModule.IRepositories;
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
        IInventoryService inventoryService
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
            var purchaseOrder = await unitOfWork.PurchaseOrderRepository.GetFirstOrDefaultAsync(po => po.Id == purchaseOrderId);
            if (purchaseOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.PurchaseOrder));
            }
            await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var line in purchaseOrder.Lines)
                {
                    await inventoryService.AddInventoryByPurchaseOrder(new AddInventoryModel
                    {
                        ProductId = line.ProductId,
                        WareHouseId = purchaseOrder.WareHouseId,
                        Quantity = line.Quantity,
                        Price = line.Price,
                        CreatedBy = User.GetUserId(),
                    });
                    await unitOfWork.InventoryTransactionRepository.AddAsync(new InventoryTransaction {
                        ProductId = line.ProductId,
                        WareHouseId = purchaseOrder.WareHouseId,
                        QuantityChange = line.Quantity,
                        UnitCost = line.Price,
                        SourceType = InventorySourceTypeEnum.PurchaseOrder,
                        SourceId = purchaseOrder.Id,
                        CreatedBy = User.GetUserId(),
                    });
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToApprovePurchaseOrder));
            }
            return NoContent();
        }
    }
}
