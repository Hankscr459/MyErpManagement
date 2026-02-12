using MyErpManagement.Core.Modules.ProductsModule.Entities;

namespace MyErpManagement.Core.Modules.InventoryModule.Entities
{
    public class InventoryPolicy
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        /// <summary>
        /// 安全庫存量
        /// </summary>
        public int SafetyStock { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual WareHouse WareHouse { get; set; } = null!;

        //public int LeadTimeDays { get; set; }
    }
}
