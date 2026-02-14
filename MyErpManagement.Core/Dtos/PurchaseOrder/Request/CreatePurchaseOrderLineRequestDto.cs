namespace MyErpManagement.Core.Dtos.PurchaseOrder.Request
{
    public class CreatePurchaseOrderLineRequestDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        /// <example>57b6e58d-53e5-f011-901d-1c697a732b9f</example>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        /// <example>10</example>
        public int Quantity { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        /// <example>3000</example>
        public Decimal Price { get; set; }
    }
}
