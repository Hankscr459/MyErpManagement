using MyErpManagement.Core.Modules.InventoryModule.Enums;

namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    public class TransferInventoryTransactionModel
    {
        public Guid ProductId { get; set; }
        public Guid FromWareHouseId { get; set; }
        public Guid ToWareHouseId { get; set; }
        public int QuantityChange { get; set; }
        public InventorySourceTypeEnum SourceType { get; set; }
        public Guid SourceId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
