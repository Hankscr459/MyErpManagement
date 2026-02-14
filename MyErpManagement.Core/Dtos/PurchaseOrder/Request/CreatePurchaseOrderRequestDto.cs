namespace MyErpManagement.Core.Dtos.PurchaseOrder.Request
{
    public class CreatePurchaseOrderRequestDto
    {
        /// <summary>
        /// 倉庫Id
        /// </summary>
        /// <example>eb8a8e42-c834-4ddb-a79a-8161115773be</example>
        public Guid WareHouseId { get; set; }
        /// <summary>
        /// 供應商Id
        /// </summary>
        /// <example>9fa4a619-eef4-4577-a6d6-4b4456cdac84</example>
        public Guid SupplierId { get; set; }
        /// <summary>
        /// 訂單日期
        /// </summary>
        /// <example>2026-02-01T00:00:00+08:00</example>
        public DateTimeOffset OrderDate { get; set; }
        /// <summary>
        /// 商品清單
        /// </summary>
        public List<CreatePurchaseOrderLineRequestDto> Lines { get; set; } = new();
    }
}
