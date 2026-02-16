namespace MyErpManagement.Core.Dtos.TransferOrder.Request
{
    public class CreateTransferOrderRequestDto
    {
        /// <summary>
        /// 訂單日期
        /// </summary>
        /// <example>2026-02-01T00:00:00+08:00</example>
        public DateTimeOffset OrderDate { get; set; }
        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }
        /// <summary>
        /// 商品清單
        /// </summary>
        public List<CreateTransferOrderLineRequestDto> Lines { get; set; } = new();
    }
}
