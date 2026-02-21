using MyErpManagement.Core.Modules.InventoryModule.Enums;

namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    public class TransferInventoryTransactionModel
    {
        public Guid ProductId { get; set; }
        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }
        public int QuantityChange { get; set; }
        /// <summary>
        /// 調貨單不用填此欄位
        /// </summary>
        public InventorySourceTypeEnum? SourceType { get; set; }
        public Guid SourceId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
