namespace MyErpManagement.Core.Dtos.PurchaseOrder.Request
{
    public class CreatePurchaseOrderRequestDto
    {
        /// <summary>
        /// 倉庫Id
        /// </summary>
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// 供應商Id
        /// </summary>
        public Guid SupplierId { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        public DateTimeOffset OrderDate { get; set; }
        /// <summary>
        /// 商品清單
        /// </summary>
        public List<CreatePurchaseOrderLineRequestDto> Lines { get; set; } = new();
    }
}
