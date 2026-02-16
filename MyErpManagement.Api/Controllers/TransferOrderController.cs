using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.Dtos.TransferOrder.Request;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.InventoryModule.Enums;
using MyErpManagement.Core.Modules.InventoryModule.IServices;
using MyErpManagement.Core.Modules.InventoryModule.Models;
using MyErpManagement.Core.Modules.OrderNoModule.Enums;
using MyErpManagement.Core.Modules.OrderNoModule.IServices;
using MyErpManagement.Core.Modules.PurchaseOrderModule.Enums;
using MyErpManagement.Core.Modules.TransferOrderModule.Entities;
using MyErpManagement.Core.Modules.TransferOrderModule.Enums;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using System.Net;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Controllers
{
    [Tags("調貨單")]
    [SwaggerControllerOrder(6)]
    public class TransferOrderController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOrderNoService orderNoService,
        IInventoryService inventoryService,
        IInventoryTransactionService inventoryTransactionService
    ) : BaseApiController
    {
        /// <summary>
        /// 新增調貨單
        /// </summary>
        /// <param name="createTransferOrderRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.TransferOrder.CreateTransferOrder.Key)]
        public async Task<ActionResult> CreatePurchaseOrder([FromBody] CreateTransferOrderRequestDto createTransferOrderRequestDto)
        {
            var transferOrder = mapper.Map<TransferOrder>(createTransferOrderRequestDto);
            transferOrder.CreatedBy = User.GetUserId();
            transferOrder.Status = TransferOrderStatusEnum.Draft;
            await unitOfWork.BeginTransactionAsync();
            try
            {
                transferOrder.OrderNo = await orderNoService.GeneratePrivateOrderNo(OrderTypeEnum.TO, createTransferOrderRequestDto.OrderDate);
                await unitOfWork.TransferOrderRepository.AddAsync(transferOrder);
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
        /// 核准調貨單
        /// </summary>
        /// <returns></returns>
        [HttpPost("{transferOrderId}/approve")]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.ApprovePurchaseOrder.Key)]
        public async Task<ActionResult> ApprovePurchaseOrder(Guid transferOrderId)
        {
            var transferOrder = await unitOfWork.TransferOrderRepository.GetFirstOrDefaultAsync(to => to.Id == transferOrderId, "Lines");
            if (transferOrder is null)
            {
                return NotFound(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Transfer));
            }
            await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var line in transferOrder.Lines)
                {
                    var transferInventoryArg = new TransferInventoryModel
                    {
                        ProductId = line.ProductId,
                        FromWareHouseId = transferOrder.FromWareHouseId,
                        ToWareHouseId = transferOrder.ToWareHouseId,
                        Quantity = line.Quantity,
                        CreatedBy = User.GetUserId(),
                    };
                    if (!await inventoryService.AddInventoryByCreateTransferOrder(transferInventoryArg))
                    {

                    }
                    //await inventoryTransactionService.AddInventoryTransaction(new AddInventoryTransactionModel
                    //{
                    //    ProductId = line.ProductId,
                    //    WareHouseId = transferOrder.FromWareHouseId,
                    //    QuantityChange = line.Quantity,
                    //    UnitCost = line.Price,
                    //    SourceType = InventorySourceTypeEnum.PurchaseOrder,
                    //    SourceId = transferOrder.Id,
                    //    CreatedBy = User.GetUserId(),
                    //});
                }
                transferOrder.Status = TransferOrderStatusEnum.Approved;
                unitOfWork.TransferOrderRepository.Update(transferOrder);
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
    }
}
