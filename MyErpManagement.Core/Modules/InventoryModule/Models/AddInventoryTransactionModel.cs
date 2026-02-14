using MyErpManagement.Core.Modules.InventoryModule.Enums;

namespace MyErpManagement.Core.Modules.InventoryModule.Models
{
    public class AddInventoryTransactionModel
    {
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public int QuantityChange { get; set; }
        public Decimal UnitCost { get; set; }
        public InventorySourceTypeEnum SourceType { get; set; }
        public Guid SourceId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
