using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.PurchaseOrder.Request;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
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
        IPurchaseOrderService purchaseOrderService
    ) : BaseApiController
    {
        /// <summary>
        /// 新增採購單
        /// </summary>
        /// <param name="createPurchaseOrderRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.PurchaseOrder.CreatePurchaseOrder.Key)]
        public async Task<ActionResult> CreateProduct([FromBody] CreatePurchaseOrderRequestDto createPurchaseOrderRequestDto)
        {
            var purchaseOrder = mapper.Map<PurchaseOrder>(createPurchaseOrderRequestDto);
            purchaseOrder.CreatedBy = User.GetUserId();
            purchaseOrder.OrderNo = await orderNoService.GeneratePrivateOrderNo(OrderTypeEnum.PO, createPurchaseOrderRequestDto.OrderDate);
            purchaseOrder.Status = PurchaseOrderStatusEnum.Draft;
            purchaseOrder.TotalPrice = purchaseOrderService.CountTotalPrice(mapper.Map<IList<PurchaseOrderCountTotalPriceModel>>(createPurchaseOrderRequestDto.Lines));
            await unitOfWork.PurchaseOrderRepository.AddAsync(purchaseOrder);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateProduct));
            }
            return NoContent();
        }
    }
}
