using MyErpManagement.Core.Modules.InventoryModule.Enums;

namespace MyErpManagement.Core.Modules.InventoryModule.Entities
{
    /// <summary>
    /// 事實紀錄
    /// </summary>
    public class InventoryTransaction
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WareHouseId { get; set; }
        public int QuantityChange { get; set; }
        /// <summary>
        /// 當時成本
        /// </summary>
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public InventorySourceTypeEnum SourceType { get; set; }
        public Guid SourceId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
